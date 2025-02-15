using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace DefaultNamespace
{
    public class SaveSystem
    {
        private static string saveFilePath = Path.Combine(Application.persistentDataPath, "quiz_data.json");
        private static QuizDataLocal _quizDataLocal;

#if UNITY_EDITOR
        [MenuItem("Tools/Open Save Folder")]
        public static void OpenSaveFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
#endif
        
        static SaveSystem()
        {
            LoadData();
        }

        public static void SaveQuestion(string question)
        {
            var hash = question.GetHashCode();
            if (!_quizDataLocal.completedQuestionHashes.Contains(hash))
            {
                _quizDataLocal.completedQuestionHashes.Add(hash);
                SaveData();
            }
        }

        public static bool IsQuestionCompleted(string question)
        {
            return _quizDataLocal.completedQuestionHashes.Contains(question.GetHashCode());
        }

        private static void SaveData()
        {
            string json = JsonUtility.ToJson(_quizDataLocal, true);
            File.WriteAllText(saveFilePath, json);
        }

        private static void LoadData()
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                _quizDataLocal = JsonUtility.FromJson<QuizDataLocal>(json);
            }
            else
            {
                _quizDataLocal = new QuizDataLocal();
            }
        }
    }
}