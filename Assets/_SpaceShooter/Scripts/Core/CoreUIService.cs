using System;
using AppBootstrap.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpaceShooter
{
    [Injectable]
    public class CoreUIService : IScreenChangeSub
    {
        [Inject] private IUIService _uiService;
        [Inject] private IPlayerLiveService _liveService;

        private const string PrefabPath = "Prefabs/Core/CoreUI";
        private GameObject _coreUI;


        [Init("Preload")]
        private void Init()
        {
            var prefab = Resources.Load<GameObject>(PrefabPath);
            var root = _uiService.GetElement("UICanvas").transform;
            _coreUI = Object.Instantiate(prefab, root);
            _uiService.AddElement(_coreUI);
            _coreUI.SetActive(false);
        }

        public void AtScreenActiveChange(string screen, bool active)
        {
            if (screen != Screens.Core)
                return;
            _coreUI.SetActive(active);
        }
    }
}