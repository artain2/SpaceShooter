using System;
using AppBootstrap.Runtime;
using UniRx;
using UnityEngine;

namespace SpaceShooter.Core.Ship
{
    [Injectable]
    public class ShipMotionService : IShipSub, IClickSub, ICoreInputAllowSub
    {
        private Transform _shipTr;
        private bool _inputAllow;
        private Vector2 _targetPos;
        private bool _isMoving = false;
        private float _moveTh = .01f;
        private float _moveSpeed = 10f;
        private CompositeDisposable _dispose = new CompositeDisposable();

        public void AtShipCreated(GameObject ship)
        {
            _shipTr = ship.transform;
        }

        public void AtShipRemoved(GameObject _)
        {
            _dispose.Clear();
            _shipTr = null;
        }

        public void AtClick(Vector2 position)
        {
            if (!_inputAllow || _shipTr == null)
                return;

            _targetPos = position;
           Observable.EveryUpdate().Subscribe(LerpShip).AddTo(_dispose);
        }

        public void AtInputAllowChange(bool inputAllow)
        {
            _inputAllow = inputAllow;
        }

        private void LerpShip(long xs)
        {
            var pos = (Vector2) _shipTr.position;
            if (IsNear(pos))
            {
                _shipTr.position = _targetPos;
                _dispose.Clear();
                return;
            }

            _shipTr.position = Vector2.Lerp(pos, _targetPos, Time.deltaTime * _moveSpeed);
        }

        private bool IsNear(Vector2 pos)
        {
            return Vector2.SqrMagnitude(_targetPos - pos) < _moveTh;
        }
    }
}