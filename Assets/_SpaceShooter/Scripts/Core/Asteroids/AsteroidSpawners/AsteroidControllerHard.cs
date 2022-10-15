using System;
using System.Collections.Generic;
using AppBootstrap.Runtime;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace SpaceShooter.Core.Asteroids
{
    [Injectable]
    public class AsteroidControllerHard: IAsteroidController
    {
        private const string PrefabPath = "Prefabs/Core/Asteroids/Asteroid_Hard";
        
        private Pool<Asteroid> _asteroidsPool;

        private float _spawnAreaX = 2f;
        private float _spawnPosY = 6f;
        private float _asteroidSpeed = -3f;

        private Dictionary<Asteroid, CompositeDisposable>
            _disposeDict = new Dictionary<Asteroid, CompositeDisposable>();
        
        [Init("Preload")]
        private void Init()
        {
            var prefab = Resources.Load<Asteroid>(PrefabPath);
            _asteroidsPool = new Pool<Asteroid>(prefab, null);
        }

        public string GetAsteroidType() => AsteroidTypes.Hard;
        
        public Asteroid SpawnAsteroid()
        {
            var asteroid = _asteroidsPool.Pop();
            var xPos = Random.Range(-_spawnAreaX, _spawnAreaX);
            var pos = new Vector2(xPos, _spawnPosY);
            var asteroidTr = asteroid.transform;
            asteroidTr.position = pos;
            var dispose = new CompositeDisposable();
            asteroidTr.UpdateAsObservable()
                .Subscribe(val => asteroidTr.transform.Translate(0f, _asteroidSpeed*Time.deltaTime, 0f))
                .AddTo(dispose);
            Observable.Timer(TimeSpan.FromSeconds(8)).Subscribe(x =>
            {
                DestroyAsteroid(asteroid, AsteroidDestroyType.Simple);
            }) .AddTo(dispose);
            _disposeDict[asteroid] = dispose;
            return asteroid;
        }
        
        public void DestroyAsteroid(Asteroid asteroid, AsteroidDestroyType adt)
        {
            _disposeDict[asteroid].Clear();
            _disposeDict.Remove(asteroid);
            _asteroidsPool.Return(asteroid);
        }

        public void Clear()
        {
            _asteroidsPool.ReturnAll();
            foreach (var disp in _disposeDict.Values)
                disp.Clear();
            _disposeDict.Clear();
        }
    }
}