using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Map
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private List<MapLevelView> levelViews;

        private Action<int> _clickAction;

        private void Start()
        {
            foreach (var view in levelViews)
            {
                var viewLevel = view.Level;
                view.OnClick += _ => _clickAction?.Invoke(viewLevel);
            }
        }

        public void SetLevelClickAction(Action<int> action)
        {
            _clickAction = action;
        }

        public void SetLastOpenLevel(int level)
        {
            foreach (var view in levelViews)
            {
                view.SetOpen(level >= view.Level);
            }
        }
    }
}