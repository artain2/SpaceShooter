using System;
using AppBootstrap.Runtime;
using SpaceShooter.Core.Asteroids;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceShooter.Core.Ship
{
    [Injectable]
    public class ShootingService : IShipSub, ICoreClearSub
    {
        private const float ShootDelay = .5f;
        private const float ProjectilesSpeed = .02f;

        private const string ProjectilePrefabPath = "Prefabs/Core/Projectiles/Blast";
        private Pool<Projectile> _projectilePool;
        private IDisposable _shootStream;
        private Transform _shipTr;
        private CompositeDisposable _dispose = new CompositeDisposable();


        [Init("Preload")]
        private void Init()
        {
            // Init pool
            var prefab = Resources.Load<Projectile>(ProjectilePrefabPath);
            _projectilePool = new Pool<Projectile>(prefab, null);
            _projectilePool.ExtendTo(5);
        }


        public void AtShipCreated(GameObject ship)
        {
            _shipTr = ship.transform;
            _shootStream = Observable.Interval(TimeSpan.FromSeconds(ShootDelay)).Subscribe(x => Shoot());
        }

        public void AtShipRemoved(GameObject ship)
        {
            _shootStream.Dispose();
        }

        private void Shoot()
        {
            var projectile = _projectilePool.Pop();
            projectile.transform.position = _shipTr.position;
            var dispose = new CompositeDisposable();
            projectile.UpdateAsObservable()
                .Subscribe(val => projectile.transform.Translate(0f, ProjectilesSpeed, 0f))
                .AddTo(dispose);
            projectile.Collider.OnTriggerEnter2DAsObservable()
                .Subscribe(AtTrigger)
                .AddTo(dispose);
            projectile.Renderer.OnBecameInvisibleAsObservable()
                .Subscribe(AtInvisible)
                .AddTo(dispose);
            _dispose.Add(dispose);

            void AtTrigger(Collider2D collider)
            {
                if (collider.gameObject.layer == LayerNames.Asteroid)
                {
                    Remove();
                }
            }

            void AtInvisible(Unit _)
            {
                Remove();
            }

            void Remove()
            {
                _dispose.Remove(dispose);
                dispose.Clear();
                _projectilePool.Return(projectile);
            }
        }

        public void AtLevelClear()
        {
            _dispose.Clear();
            _projectilePool.ReturnAll();
        }
    }
}