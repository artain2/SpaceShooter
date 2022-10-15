using UnityEngine;

namespace SpaceShooter.Core.Asteroids
{
    public interface IAsteroidController
    {
        string GetAsteroidType();
        Asteroid SpawnAsteroid();
        void DestroyAsteroid(Asteroid asteroid, AsteroidDestroyType adt);
        void Clear();
    }
}