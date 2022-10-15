using AppBootstrap.Runtime;
using UniRx;
using UnityEngine;

namespace SpaceShooter
{
    [Injectable]
    public class LevelTimeViewService : IScreenChangeSub, ICoreLevelSub
    {
        [Inject] private IUIService _uiService;
        [Inject] private ILevelTimeService _levelTimeService;

        private const string ViewUiId = "TimeView";

        private CompositeDisposable _disposable = new CompositeDisposable();
        private LevelTimeView _view;

        [Init("Postload")]
        private void Init()
        {
            _view = _uiService.GetElement<LevelTimeView>(ViewUiId);
        }

        //____ IScreenChangeSub ____________________________
        public void AtScreenActiveChange(string screen, bool active)
        {
            if (screen != Screens.Core)
                return;

            _disposable.Clear();
        }

        public void AtLevelStarted(int level)
        {
            _disposable.Clear();
            _levelTimeService.TimeLeft
                .Subscribe(timeLeft => _view.SetTime(timeLeft))
                .AddTo(_disposable);
        }
    }
}