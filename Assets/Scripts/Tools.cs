using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class Tools
    {
        #if UNITY_EDITOR
        [MenuItem("MyTools/ResetLevel")]
        public static void ResetLevel()
        {
            PlayerPrefs.DeleteKey(LevelLoader.LevelKey);
            Debug.Log("Прогресс сброшен");
        }
        #endif
  
    }
}