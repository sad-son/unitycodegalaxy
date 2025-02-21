using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.User
{
    public class UserProvider : MonoBehaviour
    {
        [SerializeField] private float duration = 1f; 
        [SerializeField] private Vector2 targetScale = new Vector2(1.5f, 1.5f);
        [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        public TMP_Text _rankText;
        public TMP_Text _healthText;
        public RectTransform _targetRect;
        
        private void Awake()
        {
            UpdateState();
            LocalDataSystem.OnRankUp += OnRankUp;
            HealthSystem.OnHealthChanged += OnHealthChanged;
            OnHealthChanged(HealthSystem.currentHealth);
        }

        private void OnDestroy()
        {
            LocalDataSystem.OnRankUp -= OnRankUp;
            HealthSystem.OnHealthChanged -= OnHealthChanged;
        }
        
        private void OnHealthChanged(int value)
        {
            _healthText.text = value.ToString();
            PulseAnimationAsync().Forget();
        }
        public async UniTaskVoid PulseAnimationAsync()
        {
            Vector2 originalScale = _targetRect.localScale;
        
            await AnimateToScaleAsync(targetScale, duration / 2);
            // Уменьшаем обратно
            await AnimateToScaleAsync(originalScale, duration / 2);
        }
        
        private async UniTask AnimateToScaleAsync(Vector2 target, float animDuration)
        {
            Vector2 startScale = _targetRect.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < animDuration)
            {
                float progress = elapsedTime / animDuration;
                float curvedProgress = curve.Evaluate(progress);
                _targetRect.localScale = Vector2.Lerp(startScale, target, curvedProgress);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            _targetRect.localScale = target;
        }
        private void OnRankUp()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            Debug.LogError($"SAD current rank {LocalDataSystem.localData.userInfo.rank}");
            _rankText.text = LocalDataSystem.localData.userInfo.rank;
        }
    }
}