using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace User
{
    public class LivesManager : MonoBehaviour
    {
        [SerializeField] private int maxLives = 5;
        [SerializeField] private float recoveryTimeInSeconds = 5;
        [SerializeField] private TextMeshProUGUI _healthText;

        [SerializeField] private float duration = 1f; 
        [SerializeField] private Vector2 targetScale = new Vector2(1.5f, 1.5f);
        [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private RectTransform _targetLivesRect;
        
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private RectTransform _targetTimerRect;
        
        private int currentLives;
        private DateTime nextLifeTime;
        private Coroutine timerCoroutine;
        private void Start()
        {
            UpdateLives(HealthSystem.currentHealth);
            LoadData();
            HealthSystem.OnHealthChanged += OnHealthChanged;
           
            RunTimer();
        }

        private void RunTimer()
        {
            if (HealthSystem.currentHealth >= maxLives)
                return;
            
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        private void OnDestroy()
        {
            HealthSystem.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int value)
        {
            UpdateLives(value);
            SaveTime();
        }

        private void UpdateLives(int value)
        {
            currentLives = value;
            SetNextTime(DateTime.Now.AddSeconds(recoveryTimeInSeconds));
            
            _healthText.text = value.ToString();
            
            PulseAnimationAsync(_targetLivesRect).Forget();
            CheckLivesRecovery();
            CheckMaxLives();
            RunTimer();
        }

        private void SetNextTime(DateTime time)
        {
            nextLifeTime = time;
        }
        private void CheckMaxLives()
        {
            if (currentLives >= maxLives)
            {
                _timerText.text = "FULL";
                if (timerCoroutine != null)
                    StopCoroutine(timerCoroutine);
                return;
            }
        }

        private async UniTaskVoid PulseAnimationAsync(RectTransform rectTransform)
        {
            Vector2 originalScale = rectTransform.localScale;
        
            await AnimateToScaleAsync(rectTransform, targetScale, duration / 2);
            await AnimateToScaleAsync(rectTransform, originalScale, duration / 2);
        }
        
        private async UniTask AnimateToScaleAsync(RectTransform rectTransform, Vector2 target, float animDuration)
        {
            Vector2 startScale = _targetLivesRect.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < animDuration)
            {
                float progress = elapsedTime / animDuration;
                float curvedProgress = curve.Evaluate(progress);
                rectTransform.localScale = Vector2.Lerp(startScale, target, curvedProgress);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            rectTransform.localScale = target;
        }

        private IEnumerator TimerCoroutine()
        {
            while (true)
            {
                UpdateTimer();
                CheckMaxLives();
                yield return new WaitForSeconds(1f);
            }
        }
        
        private void UpdateTimer()
        {
            TimeSpan timeLeft = nextLifeTime - DateTime.Now;
        
            PulseAnimationAsync(_targetTimerRect).Forget();
            if (timeLeft.TotalSeconds <= 0)
            {
                AddLife();
            }
            else
            {
                _timerText.text = string.Format("{0:00}:{1:00}", 
                    Mathf.FloorToInt((float)timeLeft.TotalMinutes), 
                    timeLeft.Seconds);
            }
        }
        
        private void LoadData()
        {
            currentLives = HealthSystem.currentHealth;
        
            string savedTime = HealthSystem.nextLifeTime;
            if (!string.IsNullOrEmpty(savedTime))
            {
                SetNextTime(DateTime.Parse(savedTime));
            }
            else
            {
                SetNextTime(DateTime.Now);
            }

            CheckLivesRecovery();
            CheckMaxLives();
            HealthSystem.SaveLives(currentLives, nextLifeTime.ToString());
        }

        private void CheckLivesRecovery()
        {
            TimeSpan timePassed = DateTime.Now - nextLifeTime;
        
            if (timePassed.TotalSeconds > 0 && currentLives < maxLives)
            {
                int livesToAdd = Mathf.Min(
                    Mathf.FloorToInt((float)timePassed.TotalSeconds / recoveryTimeInSeconds),
                    maxLives - currentLives
                );

                if (livesToAdd <= 0)
                    return;
                
                currentLives += livesToAdd;
                if (currentLives < maxLives)
                {
                    SetNextTime(nextLifeTime.AddSeconds(livesToAdd * recoveryTimeInSeconds));
                    RunTimer();
                }
            }
        }

        private void AddLife()
        {
            if (currentLives < maxLives)
            {
                SetNextTime(DateTime.Now.AddSeconds(recoveryTimeInSeconds));
                IncreaseLives();
            }
        }

        private void IncreaseLives()
        {
            HealthSystem.Increase();
            SaveTime();
        }
        
        private void SaveTime()
        {
            HealthSystem.StartRestore(nextLifeTime.ToString());
        }

        void OnApplicationQuit()
        {
           // SaveTime();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
               // SaveTime();
            }
        }
    }
}