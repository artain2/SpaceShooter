using AppBootstrap.Runtime;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    [Injectable]
    public class ExitToMapService
    {
        [Inject] private IUIService _uiService;
        [Inject] private IScreenService _screenService;
        [Inject] private IGameLoopService _gameLoopService;

        private const string ViewUiId = "CoreExitButton";

        private Button _button;

        [Init("Postload")]
        private void Init()
        {
            _button = _uiService.GetElement<Button>(ViewUiId);
            _button.OnClickAsObservable().Subscribe(x =>
            {
                _gameLoopService.ClearGameLoop();
                _screenService.LoadScreen(Screens.Map);
            });
        }
    }
}