using System;
using System.Collections.Generic;
using AppBootstrap.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceShooter.Core.Asteroids
{
    public interface ILevelDataGenerator
    {
        public LevelParameteresData Generate(int level);
    }

    [Injectable]
    public class LevelDataGenerator : ILevelDataGenerator
    {
        private int _minAsteroids = 5;
        private int _maxAsteroids = 15;
        private float _minTime = 5f;
        private float _maxTime = 10f;

        private List<(string type, float activationChance, float minWeight, float maxWeight)> _asteroidTypesParams
            = new List<(string type, float activationChance, float minWeight, float maxWeight)>()
            {
                (AsteroidTypes.Common, 1f, 1f, 5f),
                (AsteroidTypes.Hard, .8f, 1f, 4f),
                (AsteroidTypes.Snowball, .6f, 1f, 3f),
                (AsteroidTypes.Splitter, .4f, 1f, 2f),
            };

        public LevelParameteresData Generate(int level)
        {
            var result = new LevelParameteresData();
            result.Level = level;
            result.Amount = Random.Range(_minAsteroids, _maxAsteroids + 1);
            result.Seconds = Mathf.Round(Random.Range(_minTime, _maxTime));
            foreach (var param in _asteroidTypesParams)
            {
                if (param.activationChance < Random.value)
                    continue;
                var typeWeight = new LevelParameteresData.AsteroidTypeWeight()
                {
                    AsteroidID = param.type,
                    Weight = Random.Range(param.minWeight, param.maxWeight)
                };
                result.TypeWeights.Add(typeWeight);
            }

            return result;
        }
    }
}