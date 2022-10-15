using System;
using System.Collections.Generic;
using AppBootstrap.Runtime;

namespace SpaceShooter.Map
{
    public interface ILevelProgressService
    {
        public int GetLastOpenedLevel();
        public void SetLastOpenedLevel(int level);
        public int GetCurrentPlayingLevel();
        public void SetCurrentPlayingLevel(int level);
        public bool GetCompletedLevel(int level);
        public void SetLevelCompleted(int level);
    }

    [Injectable]
    public class LevelProgressService : ILevelProgressService, IDataService
    {
        private LevelProgressData _data;

        // ____ ILevelProgressService _________________________________

        public int GetLastOpenedLevel() => _data.LastOpenedLevel;

        public void SetLastOpenedLevel(int level) => _data.LastOpenedLevel = level;

        public int GetCurrentPlayingLevel() => _data.CurrentPlayingLevel;

        public void SetCurrentPlayingLevel(int level) => _data.CurrentPlayingLevel = level;
        
        public bool GetCompletedLevel(int level) => _data.CompletedLevels.Contains(level);

        public void SetLevelCompleted(int level) => _data.CompletedLevels.Add(level);

        // ____ IDataService _________________________________
        public string GetDataKey() => "LevelProgress";

        public void SetData(object data)
        {
            _data = data as LevelProgressData;
        }

        public object GetData() => _data;

        public object CreateData() => new LevelProgressData()
        {
            LastOpenedLevel = 1,
            CurrentPlayingLevel = 1,
        };


        [Serializable]
        public class LevelProgressData
        {
            public int LastOpenedLevel;
            public int CurrentPlayingLevel;
            public HashSet<int> CompletedLevels = new HashSet<int>();
        }
    }
}