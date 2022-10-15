using System;
using System.Collections.Generic;
using AppBootstrap.Runtime;
using UniRx;
using UnityEngine;

namespace SpaceShooter
{
    public interface IClickSub
    {
        void AtClick(Vector2 position);
    }

    [Injectable]
    public class InputController
    {
        [Inject] private ICameraService _cameraService;
        [Inject] private List<IClickSub> _clickSubs;

        [Init("Postload")]
        void StartRecord()
        {
            var clickStream = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0));

            clickStream.Subscribe(AtClick);
        }

        private void AtClick(long time)
        {
            var pos = (Vector2) _cameraService.Camera.ScreenToWorldPoint(Input.mousePosition);
            _clickSubs.ForEach(x=>x.AtClick(pos));
        }
    }
}