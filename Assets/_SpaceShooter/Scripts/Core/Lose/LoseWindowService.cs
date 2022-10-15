using AppBootstrap.Runtime;
using UnityEngine;

namespace SpaceShooter.Lose
{
    public interface ILoseWindowService
    {
        void ShowWindow();
    }

    [Injectable]
    public class LoseWindowService : ILoseWindowService
    {
        [Inject] private IUIService _uiService;
        [Inject] private IGameLoopService _gameLoopService;
        [Inject] private IScreenService _screenService;

        private const string PrefabPath = "Prefabs/Core/UI/LoseWindow";

        private LoseWindow _window;

        [Init("Postload")]
        private void Init()
        {
            var prefab = Resources.Load<LoseWindow>(PrefabPath);
            var root = _uiService.GetElement("UICanvas").transform;
            _window = Object.Instantiate(prefab, root);
            _uiService.AddElement(_window.gameObject);
            _window.gameObject.SetActive(false);
            _window.SetExitAction(AtExit);
            _window.SetRestartAction(AtRestart);
        }

        public void ShowWindow()
        {
            _window.Show();
        }

        private void AtExit()
        {
            _gameLoopService.ClearGameLoop();
            _screenService.LoadScreen(Screens.Map);
            _window.Hide();
        }

        private void AtRestart()
        {
            _gameLoopService.ClearGameLoop();
            _gameLoopService.StartGameLoop(1);
            _window.Hide();
        }
    }
}