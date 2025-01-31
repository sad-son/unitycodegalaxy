using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class LevelLoader : MonoBehaviour
    {
        public RectTransform canvas;
        public TextAsset jsonFile;
        
        public const string LevelKey = "CurrentLevel";
        public Quiz[] quizItems;
        public Quiz prefab;
        private Quiz _currentQuiz;
        private int currentLevel;
        private void Awake()
        {
            currentLevel = GetLevel();
            Load();
        }

        private void Load()
        {
            var index = currentLevel - 1;
            var currentIndex = index % quizItems.Length;
            Debug.Log(currentIndex);
            
            var quiz = JsonUtility.FromJson<QuizData>(jsonFile.text);
            
            var question = quiz.questions[currentIndex];
            if (!_currentQuiz)
                _currentQuiz = Instantiate(prefab, canvas);
            
            _currentQuiz.onCompleted += OnCompleted;
            _currentQuiz.Setup(question);

        }
        
        private void OnCompleted()
        {
            currentLevel += 1;
            _currentQuiz.onCompleted -= OnCompleted;
            SaveLevel(currentLevel);
            Load();
        }

        public void SaveLevel(int level)
        {
            PlayerPrefs.SetInt(LevelKey, level);
            PlayerPrefs.Save();
            Debug.Log("Уровень сохранен: " + level);
        }

        public static int GetLevel()
        {
            return PlayerPrefs.GetInt(LevelKey, 1); 
        }
    }
}