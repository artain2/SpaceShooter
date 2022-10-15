using AppBootstrap.Runtime;

namespace SpaceShooter
{
    [Injectable]
    public class GameRunner
    {
        [Inject] private IScreenService _screenService;

        [Init("Postload")]
        public void Run()
        {
            _screenService.LoadScreen(Screens.Map);
        }
    }
}