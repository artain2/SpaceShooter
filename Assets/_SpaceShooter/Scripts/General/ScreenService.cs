﻿using System.Collections.Generic;
using AppBootstrap.Runtime;

namespace SpaceShooter
{
    public interface IScreenService
    {
        void LoadScreen(string screen);
        string GetActiveScreen();
    }

    public interface IScreenChangeSub
    {
        void AtScreenActiveChange(string screen, bool active);
    }

    public static class Screens
    {
        public const string Core = "Core";
        public const string Map = "Map";
    }

    [Injectable]
    public class ScreenService : IScreenService
    {
        [Inject] private List<IScreenChangeSub> _screenScangeSubs;

        private string _activeScreen;

        public void LoadScreen(string screen)
        {
            if (!string.IsNullOrEmpty(_activeScreen))
            {
                _screenScangeSubs.ForEach(x => x.AtScreenActiveChange(_activeScreen, false));
            }

            _activeScreen = screen;
            _screenScangeSubs.ForEach(x => x.AtScreenActiveChange(screen, true));
        }

        public string GetActiveScreen() => _activeScreen;
    }
}