using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpaceShooter
{
    public class Pool 
    {
        private Transform _root;
        private GameObject _prefab;
        private List<GameObject> _inactiveList = new List<GameObject>();
        private List<GameObject> _activeList = new List<GameObject>();

        public Pool(GameObject prefab, Transform root)
        {
            _root = root;
            _prefab = prefab;
        }

        public void ExtendTo(int count)
        {
            var toAdd = count - _inactiveList.Count - _activeList.Count;
            for (var i = 0; i <= toAdd; i++)
                Extend();
        }

        public List<GameObject> GetAllActive() => _activeList;

        public GameObject Pop()
        {
            if (!_inactiveList.Any())
            {
                Extend();
            }

            var val = _inactiveList.Last();
            _inactiveList.Remove(val);
            val.SetActive(true);
            _activeList.Add(val);
            return val;
        }

        public void Return(GameObject val)
        {
            val.SetActive(false);
            _activeList.Remove(val);
            _inactiveList.Add(val);
        }

        public void ReturnAll()
        {
            _activeList.ForEach(x=>x.gameObject.SetActive(false));
            _inactiveList.AddRange(_activeList);
            _activeList.Clear();
        }

        private void Extend()
        {
            var inst = Object.Instantiate(_prefab, _root);
            inst.SetActive(false);
            _inactiveList.Add(inst);
        }
    }
}