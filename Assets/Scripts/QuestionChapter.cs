using System;
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
            _button.onClick.AddListener(OnClick);
            HealthSystem.OnHealthChanged += OnHealthChanged;
        }
        private void OnDestroy()
        {
            HealthSystem.OnHealthChanged -= OnHealthChanged;
            _button.onClick.RemoveAllListeners();
            onCompleted = null;
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
            if (LocalDataSystem.IsQuestionCompleted(_currentQuestion.title))
            {
                _image.sprite = _completedQuestionSprite;
            }
        }

        public void OnClick()
        {
            if (HealthSystem.currentHealth <= 0)
            {
                TextReminder.Instance.Notify("Not enough lives");
                return;
            }
            
            SetActiveQuestionChapters(false);
            if (_quiz == null)
                _quiz = Instantiate(LevelLoader.instance.quizPrefab, LevelLoader.instance.safeArea);
            
            _quiz.gameObject.SetActive(true);
            _quiz.Setup(_currentQuestion);
            _quiz.onCompleted += OnCompleted;
        }
        
        private void OnHealthChanged(int value)
        {
            if (value <= 0)
                CloseQuiz();
        }
        

        private void OnCompleted()
        {
            LocalDataSystem.SaveQuestion(_currentQuestion.title);
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
            _currentQuestion = _subjectChapter._questions[i + 1];
            _quiz.Setup(_currentQuestion);
        }

        private void CompleteChapter()
        {
            CloseQuiz();
        }

        private void CloseQuiz()
        {
            UpdateVisual();
            if (_quiz)
                _quiz.gameObject.SetActive(false);
            SetActiveQuestionChapters(true);
            LevelLoader.instance.questionChapters.ForEach(questionChapter => questionChapter.UpdateVisual());
            PopupHolder.currentPopupType = PopupType.QuestionChapter;
            CloseButton.instance.gameObject.SetActive(true);
        }

        private void SetActiveQuestionChapters(bool state)
        {
            LevelLoader.instance.questionChapters.ForEach(questionChapter => questionChapter.gameObject.SetActive(state));
        }
        
    }
}