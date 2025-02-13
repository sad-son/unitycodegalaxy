using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class QuizItem : MonoBehaviour
    {
        public float answerCheckedDelay;
        public TMP_Text answerText;
        public bool rightAnswer;
        public Image image;
        public Sprite greenImage;
        public Sprite redImage;
        public Sprite defaultImage;
        private Action _onComplete;
        private Button _button;
        
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(CheckAnswer);
            
            defaultImage = image.sprite;
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

        public async void CheckAnswer()
        {
            
            if (rightAnswer)
            {
                image.sprite = greenImage;
                await UniTask.Delay(TimeSpan.FromSeconds(answerCheckedDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
                _onComplete?.Invoke();
                return;
            }
            
            image.sprite = redImage;
            await UniTask.Delay(TimeSpan.FromSeconds(answerCheckedDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
            image.sprite = defaultImage;
        }
    }
}