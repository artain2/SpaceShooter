using System;
using System.Collections.Generic;
using AppBootstrap.Runtime;
using SpaceShooter.Core.Asteroids;
using SpaceShooter.Map;
using UniRx;
using UnityEngine;

namespace SpaceShooter
{
    public interface IWinService
    {
        void Win();
    }

    [Injectable]
    public class WinService : IWinService, IScreenChangeSub, ICoreLevelSub
    {
        [Inject] private IAsteroidService _asteroidService;
        [Inject] private ILevelTimeService _levelTimeService;
        [Inject] private IWinWindowService _winWindowService;
        [Inject] private IDataSaver _saver;
        [Inject] private ILevelsSequenceService _levelsSequenceService;
        [Inject] private ILevelProgressService _levelsProgressService;
        [Inject] List<ICoreInputAllowSub> _inputSubs;

        private CompositeDisposable _disposable = new CompositeDisposable();


        public void Win()
        {
            _inputSubs.ForEach(x => x.AtInputAllowChange(false));
            
            var currentLevel = _levelsProgressService.GetCurrentPlayingLevel();
            _levelsProgressService.SetLevelCompleted(currentLevel);
            
            if (!_levelsSequenceService.IsLastLevel(currentLevel))
            {
                var nextLevel = _levelsSequenceService.GetNextLevel(currentLevel);
                _levelsProgressService.SetLastOpenedLevel(nextLevel);
            }
            _saver.Save();
            _winWindowService.ShowWindow();
        }

        public void AtScreenActiveChange(string screen, bool active)
        {
            if (screen != Screens.Core)
                return;

            if (!active)
                _disposable.Clear();
        }

        public void AtLevelStarted(int level)
        {
            _disposable.Clear();
            
            var asteroidsObs = _asteroidService.AsteroidsLeft
                .Where(x => x == 0)
                .Select(x => true)
                .First();
            
            var timeObs = _levelTimeService.TimeLeft
                .Where(x => x == TimeSpan.Zero)
                .Select(_ => true)
                .First();
            
            Observable.WhenAll(asteroidsObs, timeObs)
                .Subscribe(_ => Win())
                .AddTo(_disposable);
        }
    }
}