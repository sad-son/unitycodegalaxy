﻿using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class QuestionChapter : MonoBehaviour
    {
        public TMP_Text _text;
        public Image _image;
        public Sprite _lockedQuestionSprite;
        public Sprite _completedQuestionSprite;
        public Sprite _currentQuestionSprite;
        
        private Question _currentQuestion;
        private SubjectChapter _subjectChapter;
        private Button _button;
        private static Quiz _quiz;

        public event Action onCompleted;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Select);
            HealthSystem.OnHealthChanged += OnHealthChanged;
            CloseButton.instance.onClick += OnCloseClicked;
        }

        private void OnDestroy()
        {
            if (_quiz)
                _quiz.onCompleted -= OnCompleted;
            HealthSystem.OnHealthChanged -= OnHealthChanged;
            CloseButton.instance.onClick -= OnCloseClicked;
            _button.onClick.RemoveAllListeners();
            onCompleted = null;
        }

        private void OnEnable()
        {
            UpdateVisual();
        }

        private void OnCloseClicked()
        {
            CloseQuiz();
        }

        
        public void Setup(Question question, SubjectChapter subjectChapter, Action completed)
        {
            onCompleted += completed;
            _subjectChapter = subjectChapter;
            _currentQuestion = question;
            _text.text = _currentQuestion.short_title;

            UpdateVisual();
            PopupHolder.currentPopupType = PopupType.QuestionChapter;
        }

        public void UpdateVisual()
        {
            Debug.LogError($"SAD {_currentQuestion.title} IsQuestionCompleted {LocalDataSystem.IsQuestionCompleted(_currentQuestion.title)}");
            if (LocalDataSystem.IsQuestionCompleted(_currentQuestion.title))
            {
                _image.sprite = _completedQuestionSprite;
            }
        }

        public void Select()
        {
            if (HealthSystem.currentHealth <= 0)
            {
                TextReminder.Instance.Notify("Not enough lives");
                return;
            }
            
            SetActiveQuestionChapters(false);
            _quiz = Quiz.instance;
            _quiz.gameObject.SetActive(true);
            _quiz.Setup(_currentQuestion);
            _quiz.onCompleted += OnCompleted;
        }
        
        private void OnHealthChanged(int value)
        {
            if (value <= 0)
                CloseQuiz();
        }
        

        private void OnCompleted(string title)
        {
            if (title != _currentQuestion.title)
                return;
            
            Debug.Log($"save {_currentQuestion.title}");

            onCompleted?.Invoke();
            for (int i = 0; i < _subjectChapter._questions.Count; i++)
            {
                var question = _subjectChapter._questions[i];
                if (question.Equals(_currentQuestion))
                {
                    if (i + 1 >= _subjectChapter._questions.Count)
                    {
                        CompleteChapter();
                        return;
                    }

                    NextQuestion(i);
                    break;
                }
            }
        }

        private void NextQuestion(int i)
        {
            var nextQuestion = _subjectChapter._questions[i + 1];
            
            _quiz.onCompleted -= OnCompleted;

            var nextChapter =
                LevelLoader.instance.questionChapters.FirstOrDefault(chapter =>
                    chapter._currentQuestion.title == nextQuestion.title);
            nextChapter.Select();
        }

        private void CompleteChapter()
        {
            _quiz.onCompleted -= OnCompleted;
            CloseQuiz();
            CloseButton.instance.CloseQuestionChapter();
        }

        private void CloseQuiz()
        {
            if (PopupHolder.currentPopupType != PopupType.Quiz)
                return;
            
            if (_quiz)
                _quiz.SetActive(false);
            SetActiveQuestionChapters(true);
            UpdateVisual();
            LevelLoader.instance.questionChapters.ForEach(questionChapter => questionChapter.UpdateVisual());
            PopupHolder.currentPopupType = PopupType.QuestionChapter;
        }

        private void SetActiveQuestionChapters(bool state)
        {
            LevelLoader.instance.questionChapters.ForEach(questionChapter => questionChapter.gameObject.SetActive(state));
        }
        
    }
}