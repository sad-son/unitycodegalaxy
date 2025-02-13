using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class QuestionChapter : MonoBehaviour
    {
        public TMP_Text _text;
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
            for (int i = 0; i < _subjectChapter._questions.Count; i++)
            {
                var question = _subjectChapter._questions[i];
                if (question.Equals(_currentQuestion))
                {
                    if (i + 1 >= _subjectChapter._questions.Count)
                    {
                        CompleteSubject();
                    }
                    
                    _quiz.Setup(_subjectChapter._questions[i + 1]);
                }
            }
        }

        private void CompleteSubject()
        {
            _quiz.gameObject.SetActive(false);
            SetActiveQuestionChapters(true);
        }

        private void SetActiveQuestionChapters(bool state)
        {
            _subjectChapter._questionChapters.ForEach(questionChapter => questionChapter.gameObject.SetActive(state));
        }
    }
}