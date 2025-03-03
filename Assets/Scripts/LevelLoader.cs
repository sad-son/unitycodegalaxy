using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DefaultNamespace.Json;
using Newtonsoft.Json;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader instance;
        
        public RectTransform canvas;
        public RectTransform safeArea;
        public TextAsset jsonFile;
        
        public const string LevelKey = "CurrentLevel";
        public Quiz[] quizItems;
        public Quiz quizPrefab;
        public RectTransform rankViewContent;
        public RankChapter rankViewPrefab;
        public SubjectChapter subjectChapterPrefab;
        public QuestionChapter questionChapterPrefab;
        
        public Quiz currentQuiz;
        public List<RankChapter> rankChapters = new();
        public List<SubjectChapter> subjectChapters = new();
        [FormerlySerializedAs("_questionChapters")] public List<QuestionChapter> questionChapters = new();
        private int currentLevel;

        private void Awake()
        {
            instance = this;
        }

        public void Load()
        {
            currentLevel = GetLevel();
            var index = currentLevel - 1;
            var currentIndex = index % quizItems.Length;
            Debug.Log(currentIndex);
            
            var quizСontainer = JsonConvert.DeserializeObject<QuizContainer>(jsonFile.text);
            foreach (var quizData in quizСontainer.quizzes)
            {
                var instance = Instantiate(rankViewPrefab, rankViewContent);
                rankChapters.Add(instance);
                
                instance.Setup(this, quizData, subjectChapterPrefab);
            }
        }

        [Button]
        private void QuestionsCount()
        {
            var quizСontainer = JsonConvert.DeserializeObject<QuizContainer>(jsonFile.text);
            int count = 0;
            foreach (var quizData in quizСontainer.quizzes)
            {
                foreach (var questions in quizData.subjects.Values)
                {
                    count += questions.Count;
                }
            }
            
            Debug.Log($"total questions: {count}");
        }
        
        [Button]
        private void DuplicateSearch()
        {
            var quizСontainer = JsonConvert.DeserializeObject<QuizContainer>(jsonFile.text);
            var result = new StringBuilder();
    
            foreach (var quizData in quizСontainer.quizzes)
            {
                foreach (var questions in quizData.subjects.Values)
                {
                    foreach (var question in questions)
                    {
                        var thisQuestionCount = quizСontainer.quizzes
                            .SelectMany(quiz => quiz.subjects.Values)
                            .SelectMany(questionsSearch => questionsSearch)
                            .Count(searchQuestion => searchQuestion.title == question.title);

                        if (thisQuestionCount > 1)
                        {
                            result.AppendLine($"Duplicate: {question.title} count: {thisQuestionCount}");
                        }
                    }
                }
            }
            
            Debug.Log($"duplicates: {result}");
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