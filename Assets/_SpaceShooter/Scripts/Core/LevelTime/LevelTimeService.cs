using System;
using System.Collections.Generic;
using AppBootstrap.Runtime;
using SpaceShooter.Core.Asteroids;
using SpaceShooter.Lose;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpaceShooter
{
    public interface ILevelTimeService
    {
        ReactiveProperty<TimeSpan> TimeLeft { get; }
    }

    public interface ILevelTimerCompleteSub
    {
        void AtLevelTimeComplete();
    }

    [Injectable]
    public class LevelTimeService : ICoreLevelSub, IScreenChangeSub, ILoseService, ILevelTimeService
    {
        [Inject] private ILevelDataService _dataService;
        [Inject] private IWinService _winService;
        [Inject] private List<ILevelTimerCompleteSub> _timeCompleteSubs;

        private CompositeDisposable _dispose = new CompositeDisposable();

        public ReactiveProperty<TimeSpan> TimeLeft { get; private set; } =
            new ReactiveProperty<TimeSpan>(TimeSpan.Zero);


        //____ ICoreLevelSub ____________________________
        public void AtLevelStarted(int level)
        {
            _dataService.TryGetLevelData(level, out var data);
            var timeSpan = TimeSpan.FromSeconds(data.Seconds);
            TimeLeft.Value = timeSpan;
            Observable.Timer(timeSpan).Subscribe(v =>
            {
                TimeLeft.Value = TimeSpan.Zero;
                _timeCompleteSubs.ForEach(x => x.AtLevelTimeComplete());
                _dispose.Clear();
            }).AddTo(_dispose);
            var sec = TimeSpan.FromSeconds(1f);
            Observable.Interval(sec).Subscribe(passed =>
            {
                TimeLeft.Value -= sec;
            }).AddTo(_dispose);
        }

        //____ IScreenChangeSub ____________________________
        public void AtScreenActiveChange(string screen, bool active)
        {
            if (screen != Screens.Core)
                return;
            if (!active)
                _dispose.Clear();
        }

        //____ ILoseService ____________________________
        public void AtLose()
        {
            _dispose.Clear();
        }
    }
}