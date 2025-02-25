using System;
using System.IO;
using DefaultNamespace.User;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace DefaultNamespace
{
    public class LocalDataSystem
    {
        public static event Action OnRankUp;
        
        public static LocalData localData => _localData;
        private static string saveFilePath = Path.Combine(Application.persistentDataPath, "local_data.json");
        private static LocalData _localData;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ResetStaticValues()
        {
            LoadData(); // Сброс при старте сцены
        }
#if UNITY_EDITOR
        [MenuItem("Tools/Open Save Folder")]
        public static void OpenSaveFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
#endif
        
        static LocalDataSystem()
        {
            LoadData();
        }

        public static void SaveQuestion(string question)
        {
            var hash = question.GetHashCode();
            if (!_localData.quizData.completedQuestionHashes.Contains(hash))
            {
                _localData.quizData.completedQuestionHashes.Add(hash);
                SaveData();
            }
        }

        public static bool IsQuestionCompleted(string question)
        {
            return _localData.quizData.completedQuestionHashes.Contains(question.GetHashCode());
        }

        public static string SetRank(string rank)
        {
            _localData.userInfo.rank = rank;
            Debug.LogError($"SAD NextRank {_localData.userInfo.rank}");
            SaveData();
            OnRankUp?.Invoke();
            return _localData.userInfo.rank;
        }
        
        public static string NextRank()
        {
            _localData.userInfo.rank = _localData.userInfo.rank.NextRank();
            Debug.LogError($"SAD NextRank {_localData.userInfo.rank}");
            SaveData();
            OnRankUp?.Invoke();
            return _localData.userInfo.rank;
        }
        
        public static void SaveData()
        {
            string json = JsonUtility.ToJson(_localData, true);
            File.WriteAllText(saveFilePath, json);
        }

        private static void LoadData()
        {
            _localData = null;
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                _localData = JsonUtility.FromJson<LocalData>(json);
            }
            else
            {
                _localData = new LocalData();
            }
        }
    }
}