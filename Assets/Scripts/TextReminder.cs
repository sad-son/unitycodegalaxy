using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class TextReminder : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _duration;
        
        public static TextReminder Instance { get; private set; }
        
        private RectTransform _rectTransform;
        private TMP_Text _text;
        private float _endTime;
        private Vector3 _originalPosition;
        
        private void Awake()
        {
            Instance = this;
            _rectTransform = GetComponent<RectTransform>();
            _text = GetComponent<TMP_Text>();
            _originalPosition = _rectTransform.anchoredPosition;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _endTime = Time.time + _duration;
        }

        public void Notify(string text)
        {
            _text.text = text;
            gameObject.SetActive(true);
        }
        
        private void OnDisable()
        {
            _rectTransform.anchoredPosition = _originalPosition;
        }

        private void Update()
        {
            if (Time.time >= _endTime)
            {
                gameObject.SetActive(false);
                return;
            }
            
            _rectTransform.anchoredPosition += Vector2.up * (Time.deltaTime * _speed);
        }
    }
}