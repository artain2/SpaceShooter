using System.Collections.Generic;
using System.Linq;
using AppBootstrap.Runtime;
using UnityEngine;

namespace SpaceShooter.Map
{
    public interface ILevelsSequenceService
    {
        bool IsLastLevel(int levelId);
        int GetNextLevel(int levelId);
    }

    [Injectable]
    public class LevelsSequenceService : ILevelsSequenceService
    {
        private List<LevelInfo> levels = new List<LevelInfo>()
        {
            new LevelInfo(1),
            new LevelInfo(2),
            new LevelInfo(3),
        };

        public bool IsLastLevel(int levelId) => levels.Last().LevelId == levelId;

        public int GetNextLevel(int levelId)
        {
            for (var i = 0; i < levels.Count - 1; i++)
            {
                if (levels[i].LevelId == levelId)
                {
                    return levels[i + 1].LevelId;
                }
            }

            Debug.LogError($"Next level for {levelId} is out of range");
            return 0;
        }

        private class LevelInfo
        {
            public int LevelId;

            public LevelInfo(int levelId)
            {
                LevelId = levelId;
            }
        }
    }
}