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
        private Action _onRightAnswer;
        private Action _onComplete;
        private Button _button;
        private string _question;
        
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

        public void Setup(string question, Answer answer, Action onComplete)
        {
            _question = question;
            answerText.text = answer.answer;
            rightAnswer = answer.right;
            _onComplete = onComplete;
            image.sprite = defaultImage;
        }

        public async void CheckAnswer()
        {
            if (rightAnswer)
            {
                image.sprite = greenImage;
                SoundManager.Instance.PlayCorrectAnswer();
                LocalDataSystem.SaveQuestion(_question);
                _onRightAnswer?.Invoke();
                await UniTask.Delay(TimeSpan.FromSeconds(answerCheckedDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
                _onComplete?.Invoke();
                return;
            }

            HealthSystem.Decrease();
            SoundManager.Instance.PlayWrongAnswer();
            image.sprite = redImage;
            await UniTask.Delay(TimeSpan.FromSeconds(answerCheckedDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
            image.sprite = defaultImage;
            
            AdsManager.instance.LoadInterstitial();
        }
    }
}