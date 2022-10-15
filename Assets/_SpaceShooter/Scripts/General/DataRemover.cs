using System.IO;
using UnityEngine;

namespace SpaceShooter
{
    public class DataRemover
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Clear save")]
#endif
        public static void DeleteSave()
        {
            if (!File.Exists(DataService.DataFilePath))
            {
                return;
            }

            File.Delete(DataService.DataFilePath);
            Debug.Log("Save cleared!");
        }
    }
}