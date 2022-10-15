using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Core.Asteroids
{
    [Serializable]
    public class LevelParameteresData
    {
        [SerializeField] private int level;
        [SerializeField] private float seconds;
        [SerializeField] private int asteroidsAmount;
        [SerializeField] private List<AsteroidTypeWeight> typeWeights = new List<AsteroidTypeWeight>();

        public int Level
        {
            get => level;
            set => level = value;
        }
        
        public float Seconds
        {
            get => seconds;
            set => seconds = value;
        }

        public int Amount
        {
            get => asteroidsAmount;
            set => asteroidsAmount = value;
        }

        public List<AsteroidTypeWeight> TypeWeights
        {
            get => typeWeights;
            set => typeWeights = value;
        }


        [Serializable]
        public class AsteroidTypeWeight : IHasWeight
        {
            [SerializeField] private string asteroidId;
            [SerializeField] private float weight;


            public string AsteroidID
            {
                get => asteroidId;
                set => asteroidId = value;
            }

            public float Weight
            {
                get => weight;
                set => weight = value;
            }
        }
    }
}