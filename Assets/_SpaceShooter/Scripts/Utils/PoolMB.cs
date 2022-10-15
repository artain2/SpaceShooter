using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpaceShooter
{
    public class Pool<T> where T : MonoBehaviour
    {
        private Transform _root;
        private T _prefab;
        private List<T> _inactiveList = new List<T>();
        private List<T> _activeList = new List<T>();

        public Pool(T prefab, Transform root)
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

        public List<T> GetAllActive() => _activeList;

        public T Pop()
        {
            if (!_inactiveList.Any())
            {
                Extend();
            }

            var val = _inactiveList.Last();
            _inactiveList.Remove(val);
            val.gameObject.SetActive(true);
            _activeList.Add(val);
            return val;
        }

        public void Return(T val)
        {
            val.gameObject.SetActive(false);
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
            inst.gameObject.SetActive(false);
            _inactiveList.Add(inst);
        }
    }
}