using AppBootstrap.Runtime;
using UnityEngine;

namespace SpaceShooter
{
    public interface ICameraService
    {
        Camera Camera { get; }
    }

    [Injectable]
    public class CameraService : ICameraService
    {
        private const string CameraPrefabPath = "Prefabs/General/Camera";

        public Camera Camera { get; private set; }

        [Init("Preload")]
        private void CreateCamera()
        {
            var prefab = Resources.Load<Camera>(CameraPrefabPath);
            Camera = GameObject.Instantiate(prefab);
        }
    }
}