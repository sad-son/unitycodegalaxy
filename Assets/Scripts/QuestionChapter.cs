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
        private Quiz _quiz;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
        
        public void Setup(Question question, SubjectChapter subjectChapter)
        {
            _subjectChapter = subjectChapter;
            _currentQuestion = question;
            _text.text = _currentQuestion.short_title;

            UpdateVisual();
            PopupHolder.currentPopupType = PopupType.QuestionChapter;
        }

        private void UpdateVisual()
        {
            if (SaveSystem.IsQuestionCompleted(_currentQuestion.title))
            {
                _image.sprite = _completedQuestionSprite;
            }
        }

        public void OnClick()
        {
            SetActiveQuestionChapters(false);
            _quiz = Instantiate(LevelLoader.Instance.quizPrefab, LevelLoader.Instance.canvas);
            _quiz.Setup(_currentQuestion);
            _quiz.onCompleted += OnCompleted;
        }
        
        private void OnCompleted()
        {
            SaveSystem.SaveQuestion(_currentQuestion.title);
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
            UpdateVisual();
            _quiz.gameObject.SetActive(false);
            SetActiveQuestionChapters(true);
            PopupHolder.currentPopupType = PopupType.QuestionChapter;
            CloseButton.instance.gameObject.SetActive(true);
        }

        private void SetActiveQuestionChapters(bool state)
        {
            LevelLoader.Instance.questionChapters.ForEach(questionChapter => questionChapter.gameObject.SetActive(state));
        }
        
    }
}