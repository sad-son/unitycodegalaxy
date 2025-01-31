using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class Tools
    {
        [MenuItem("MyTools/ResetLevel")]
        public static void SayHello()
        {
            PlayerPrefs.DeleteKey(LevelLoader.LevelKey);
            Debug.Log("Прогресс сброшен");
        }
    }
}