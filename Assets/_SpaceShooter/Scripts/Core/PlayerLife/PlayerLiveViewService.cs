using AppBootstrap.Runtime;
using UniRx;
using UnityEngine;

namespace SpaceShooter
{
    public interface IPlayerLiveViewService
    {
        void SetAmount(int amount);
    }

    [Injectable]
    public class PlayerLiveViewService : IPlayerLiveViewService
    {
        [Inject] private IUIService _uiService;
        [Inject] private IPlayerLiveService _liveService;

        private const string ViewUiId = "LifeView";

        private LiveView _view;

        [Init("Postload")]
        private void Init()
        {
            _view= _uiService.GetElement<LiveView>(ViewUiId);
            _liveService.CurrentLives.Subscribe(SetAmount);
        }

        public void SetAmount(int amount)
        {
            _view.SetAmount($"{amount}");
        }

    }
}