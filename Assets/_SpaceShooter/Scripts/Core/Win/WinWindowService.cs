using AppBootstrap.Runtime;
using SpaceShooter.Map;
using UnityEngine;

namespace SpaceShooter
{
    public interface IWinWindowService
    {
        void ShowWindow();
    }

    [Injectable]
    public class WinWindowService : IWinWindowService
    {
        private const string WindowPrefabPath = "Prefabs/Core/UI/WinWindow";

        [Inject] private IUIService _uiService;
        [Inject] private IScreenService _screenService;
        [Inject] private IGameLoopService _gameLoopService;

        private WinWindow _winWindow;
        
        
        [Init("Postload")]
        private void Init()
        {
            var prefab = Resources.Load<WinWindow>(WindowPrefabPath);
            var root = _uiService.GetElement("UICanvas").transform;
            _winWindow = Object.Instantiate(prefab, root);
            _uiService.AddElement(_winWindow.gameObject);
            _winWindow.gameObject.SetActive(false);
            _winWindow.SetOkAction(AtOk);
        }
        
        public void ShowWindow()
        {
            _winWindow.Show();
        }

        private void AtOk()
        {
            _winWindow.Hide();
            _gameLoopService.ClearGameLoop();
            _screenService.LoadScreen(Screens.Map);
        }
    }
}