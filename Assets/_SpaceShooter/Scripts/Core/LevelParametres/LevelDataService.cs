using System.Linq;
using AppBootstrap.Runtime;
using UnityEngine;

namespace SpaceShooter.Core.Asteroids
{
    public interface ILevelDataService
    {
        bool TryGetLevelData(int level, out LevelParameteresData data);
        void SetLevelData(LevelParameteresData data);
    }

    [Injectable]
    public class LevelDataService : ILevelDataService, IDataService
    {
        private AllLevelsParametersData _levelParametersData;

        public bool TryGetLevelData(int level, out LevelParameteresData data)
        {
            data = _levelParametersData.levelsData.FirstOrDefault(x => x.Level == level);
            return data != null;
        }

        public void SetLevelData(LevelParameteresData data)
        {
            var contained = _levelParametersData.levelsData.FirstOrDefault(x => x.Level == data.Level);
            if (contained!=null)
            {
                _levelParametersData.levelsData.Remove(contained);
                Debug.LogError($"LevelParams >>> Already saved {data.Level}");
            }

            _levelParametersData.levelsData.Add(data);
        }

        // ____ IDataService _____________________________________

        public string GetDataKey() => "LevelParams";

        public void SetData(object data)
        {
            _levelParametersData = data as AllLevelsParametersData;
        }

        public object GetData()
        {
            return _levelParametersData;
        }

        public object CreateData()
        {
            return new AllLevelsParametersData();
        }
    }
}