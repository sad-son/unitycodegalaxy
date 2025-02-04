using System;
using DefaultNamespace.Json;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class LevelLoader : MonoBehaviour
    {
        public RectTransform canvas;
        public TextAsset jsonFile;
        
        public const string LevelKey = "CurrentLevel";
        public Quiz[] quizItems;
        public Quiz quizPrefab;
        public RectTransform rankViewContent;
        public RankChapter rankViewPrefab;
        public SubjectChapter subjectChapter;
        
        public Quiz currentQuiz;
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
            
            var quizСontainer = JsonConvert.DeserializeObject<QuizContainer>(jsonFile.text);
            foreach (var quizData in quizСontainer.quizzes)
            {
                Debug.LogError($"SAD {quizData.subjects}");
                Instantiate(rankViewPrefab, rankViewContent).Setup(this, quizData, subjectChapter);
            }
            /*var question = quiz.questions[currentIndex];
            if (!_currentQuiz)
                _currentQuiz = Instantiate(quizPrefab, canvas);
            
            _currentQuiz.onCompleted += OnCompleted;
            _currentQuiz.Setup(question);*/

        }
        
        private void OnCompleted()
        {
            currentLevel += 1;
            currentQuiz.onCompleted -= OnCompleted;
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