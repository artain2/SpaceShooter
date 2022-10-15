using System.Collections.Generic;
using AppBootstrap.Runtime;
using UnityEngine;

namespace SpaceShooter.Core.Ship
{
    public interface IShipSub
    {
        public void AtShipCreated(GameObject ship);
        public void AtShipRemoved(GameObject ship);
    }

    public interface IShipProvider
    {
        void CreateShip();
        void RemoveShip();
    }

    [Injectable]
    public class ShipProvider : IShipProvider, ICoreClearSub
    {
        [Inject] private List<IShipSub> _shipSubs;

        private const string ShipPrefabPath = "Prefabs/Core/Ships/Ship";
        private GameObject _shipPrefab;
        private GameObject _shipInstance;

        [Init("Preload")]
        private void LoadPrefab()
        {
            _shipPrefab = Resources.Load<GameObject>(ShipPrefabPath);
        }

        public void CreateShip()
        {
            _shipInstance = Object.Instantiate(_shipPrefab);
            _shipSubs.ForEach(x => x.AtShipCreated(_shipInstance));
        }

        public void RemoveShip()
        {
            _shipSubs.ForEach(x => x.AtShipRemoved(_shipInstance));
            Object.Destroy(_shipInstance);
            _shipInstance = null;
        }

        public void AtLevelClear()
        {
            RemoveShip();
        }
    }
}