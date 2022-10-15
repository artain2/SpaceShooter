using AppBootstrap.Runtime;
using UnityEngine;

namespace SpaceShooter
{
    [Injectable]
    public class UIRootService
    {
        [Inject] private ICameraService _cameraService;
        [Inject] private IUIService _uiService;

        private const string PrefabPath = "Prefabs/General/UICanvas";

        [Init("Preload")]
        private void Init()
        {
            var canvas = Resources.Load<Canvas>(PrefabPath);
            var inst = Object.Instantiate(canvas);
            inst.worldCamera = _cameraService.Camera;
            _uiService.AddElement(inst.gameObject);
        }
    }
}