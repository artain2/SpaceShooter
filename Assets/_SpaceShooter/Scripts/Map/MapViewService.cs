using AppBootstrap.Runtime;
using UnityEngine;

namespace SpaceShooter.Map
{
    public interface IMapViewService
    {
        void Show();
        void Hide();
    }

    [Injectable]
    public class MapViewService : IMapViewService, IScreenChangeSub
    {
        [Inject] private IScreenService _screenService;
        [Inject] private IGameLoopService _gameLoopService;
        [Inject] private ILevelProgressService _levelProgressService;

        private const string MapPrefabPath = "Prefabs/Map/Map";
        private MapView _view;

        [Init("Preload")]
        private void Init()
        {
            var prefab = Resources.Load<MapView>(MapPrefabPath);
            _view = Object.Instantiate(prefab);
            _view.gameObject.SetActive(false);
            _view.SetLevelClickAction(level =>
            {
                if (level > _levelProgressService.GetLastOpenedLevel())
                {
                    return;
                }

                _screenService.LoadScreen(Screens.Core);
                _gameLoopService.StartGameLoop(level);
                _levelProgressService.SetCurrentPlayingLevel(level);
            });
        }


        public void Show()
        {
            _view.gameObject.SetActive(true);
            _view.SetLastOpenLevel(_levelProgressService.GetLastOpenedLevel());
        }

        public void Hide()
        {
            _view.gameObject.SetActive(false);
        }

        public void AtScreenActiveChange(string screen, bool active)
        {
            if (screen != Screens.Map)
                return;
            
            if (active)
                Show();
            else
                Hide();
        }
    }
}