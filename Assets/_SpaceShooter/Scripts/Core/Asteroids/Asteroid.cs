using UnityEngine;

namespace SpaceShooter.Core.Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private Collider2D collider;
        [SerializeField] private Renderer renderer;

        public Renderer Renderer => renderer;
        public Collider2D Collider => collider;
    }
}