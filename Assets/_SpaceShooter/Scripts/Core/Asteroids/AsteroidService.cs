using System;
using System.Collections.Generic;
using System.Linq;
using AppBootstrap.Runtime;
using SpaceShooter.Core.Ship;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceShooter.Core.Asteroids
{
    public interface IAsteroidService
    {
        ReactiveProperty<int> AsteroidsLeft { get;  } 
    }

    [Injectable]
    public class AsteroidService : ICoreLevelSub, ICoreClearSub, IAsteroidService
    {
        [Inject] private ILevelDataService _dataService;
        [Inject] private ILevelDataGenerator _dataGenerator;
        [Inject] private IDataSaver _saver;
        [Inject] private List<IAsteroidController> _controllers;
        [Inject] private List<IPlayerHitSub> _playerHitSubs;
        
        private const float StartSpawnDelay = 1f;

        private List<IDisposable> _asteroidSpawns = new List<IDisposable>();
        private CompositeDisposable _dispose = new CompositeDisposable();
        private CompositeDisposable _spawnDispose = new CompositeDisposable();

        public ReactiveProperty<int> AsteroidsLeft { get; private set; } = new ReactiveProperty<int>(0);

        public void AtLevelStarted(int level)
        {
            if (!_dataService.TryGetLevelData(level, out var data))
            {
                data = _dataGenerator.Generate(level);
                _dataService.SetLevelData(data);
                _saver.Save();
            }

            Debug.Log($"Asteroids on level {level}: " +
                      $"Amount {data.Amount}, " +
                      $"used types: {string.Join(",", data.TypeWeights.Select(x => x.AsteroidID))}");

            AsteroidsLeft.Value = data.Amount;
            _asteroidSpawns.Clear();
            var levelTime = data.Seconds;
            for (var i = 0; i < data.Amount; i++)
            {
                var asteroidType = Weights.GetWeightObject(data.TypeWeights).AsteroidID;
                var spawnTime = Random.Range(StartSpawnDelay, levelTime);
                var spawn = Observable.Timer(TimeSpan.FromSeconds(spawnTime))
                    .Subscribe(x => CreateAsteroid(asteroidType)).AddTo(_spawnDispose);
                _asteroidSpawns.Add(spawn);
            }
        }

        private Asteroid CreateAsteroid(string type)
        {
            var localDispose = new CompositeDisposable();

            var controller = _controllers.First(x => x.GetAsteroidType() == type);
            var asteroid = controller.SpawnAsteroid();
            asteroid.Collider.OnTriggerEnter2DAsObservable().Subscribe(AtTrigger).AddTo(localDispose);
            asteroid.Renderer.OnBecameInvisibleAsObservable().Subscribe(AtOutOfScreen).AddTo(localDispose);
            _dispose.Add(localDispose);
            return asteroid;

            void AtTrigger(Collider2D collider)
            {
                _dispose.Remove(localDispose);
                localDispose.Clear();
                AsteroidsLeft.Value -= 1;
                if (collider.gameObject.layer == LayerNames.Projectile)
                {
                    controller.DestroyAsteroid(asteroid, AsteroidDestroyType.Killed);
                    return;
                }

                if (collider.gameObject.layer == LayerNames.Player)
                {
                    controller.DestroyAsteroid(asteroid, AsteroidDestroyType.HitPlayer);
                    _playerHitSubs.ForEach(x => x.AtHit());
                }
            }

            void AtOutOfScreen(Unit _)
            {
                _dispose.Remove(localDispose);
                localDispose.Clear();
                controller.DestroyAsteroid(asteroid, AsteroidDestroyType.Simple);
                AsteroidsLeft.Value -= 1;
            }
        }

        public void AtLevelClear()
        {
            _dispose.Clear();
            foreach (var controller in _controllers) 
                controller.Clear();
            _spawnDispose.Clear();
        }
    }

    public enum AsteroidDestroyType
    {
        Killed,
        HitPlayer,
        Simple,
    }
}