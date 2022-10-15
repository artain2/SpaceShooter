using System;
using System.Collections.Generic;
using AppBootstrap.Runtime;
using SpaceShooter.Core.Ship;

namespace SpaceShooter
{
    public interface IGameLoopService
    {
        void StartGameLoop(int level);
        void ClearGameLoop();
    }

    public interface ICoreInputAllowSub
    {
        void AtInputAllowChange(bool inputAllow);
    }

    public interface ICoreLevelSub
    {
        void AtLevelStarted(int level);
    }

    public interface ICoreClearSub
    {
        void AtLevelClear();
    }

    [Injectable]
    public class GameLoopService : IGameLoopService
    {
        [Inject] private IShipProvider _shipProvider;
        [Inject] private List<ICoreInputAllowSub> _coreInputAllowSubs;
        [Inject] private List<ICoreLevelSub> _levelStartSubs;
        [Inject] private List<ICoreClearSub> _clearSubs;

        public void StartGameLoop(int level)
        {
            _shipProvider.CreateShip();

            _coreInputAllowSubs.ForEach(x => x.AtInputAllowChange(true));
            _levelStartSubs.ForEach(x => x.AtLevelStarted(level));
        }

        public void ClearGameLoop()
        {
            _clearSubs.ForEach(x => x.AtLevelClear());
        }
    }
}