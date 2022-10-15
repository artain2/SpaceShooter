using System;
using AppBootstrap.Runtime;
using SpaceShooter.Core.Ship;
using SpaceShooter.Lose;
using UniRx;

namespace SpaceShooter
{
    public interface IPlayerLiveService
    {
        ReactiveProperty<int> CurrentLives { get; }
    }

    [Injectable]
    public class PlayerLiveService : ICoreLevelSub, IPlayerHitSub, IPlayerLiveService
    {
        [Inject] private ILoseService _loseService;
        [Inject] private IShipProvider _shipProvider;

        private const int MaxLives = 3;

        public ReactiveProperty<int> CurrentLives { get; private set; } = new ReactiveProperty<int>();

        public void AtLevelStarted(int level)
        {
            CurrentLives.Value = MaxLives;
        }

        public void AtHit()
        {
            CurrentLives.Value -= 1;
            if (CurrentLives.Value == 0)
            {
                _shipProvider.RemoveShip();
                Observable.Timer(TimeSpan.FromSeconds(.6f)).Subscribe(x => _loseService.AtLose());
                ;
            }
        }
    }
}