using System.Collections.Generic;
using AppBootstrap.Runtime;
using UnityEngine;

namespace SpaceShooter.Lose
{
    public interface ILoseService
    {
        void AtLose();
    }

    public interface ILoseSub
    {
        void AtLose();
    }

    [Injectable]
    public class LoseService : ILoseService
    {
        [Inject] List<ICoreInputAllowSub> _inputSubs;
        [Inject] ILoseWindowService _windowService;
        [Inject] List<ILoseSub> _loseSubs;

        public void AtLose()
        {
            _inputSubs.ForEach(x => x.AtInputAllowChange(false));
            _loseSubs.ForEach(x => x.AtLose());
            _windowService.ShowWindow();
        }
    }
}