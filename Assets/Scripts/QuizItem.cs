using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class QuizItem : MonoBehaviour
    {
        public TMP_Text answerText;
        public bool rightAnswer;
        private Action _onComplete;
        private Button _button;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(CheckAnswer);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(CheckAnswer);
            _onComplete = null;
        }

        public void Setup(Answer answer, Action onComplete)
        {
            answerText.text = answer.answer;
            rightAnswer = answer.right;
            _onComplete = onComplete;
        }

        public void CheckAnswer()
        {
            if (rightAnswer)
            {
                _onComplete?.Invoke();
            }
        }
    }
}