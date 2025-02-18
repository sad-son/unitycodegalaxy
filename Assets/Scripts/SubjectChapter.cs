using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.User;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class SubjectChapter : MonoBehaviour
    {
        public Dictionary<string, List<Question>> _subjects;
        [FormerlySerializedAs("_text")] public TMP_Text _title;
        public TMP_Text _rankText;
        public List<Question> _questions;
        public Image _image;
        public Sprite _availableSprite;
        public Sprite _lockedSprite;
        public Sprite _completedSprite;
        
        
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            LocalDataSystem.OnRankUp += OnRankUp;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
            LocalDataSystem.OnRankUp -= OnRankUp;
        }
        
        private void OnRankUp()
        {
            UpdateState();
        }
        
        private void OnEnable()
        {
            CheckRankUp();
        }

        private void CheckRankUp()
        {
            Debug.Log($"SAD CheckRankUp {_title.text} AllQuestionsCompleted() {AllQuestionsCompleted()}");
            if (AllQuestionsCompleted())
            {
                _image.sprite = _completedSprite;
                _button.interactable = true;
                if (LevelLoader.Instance.subjectChapters.Where(chapter => chapter._rankText.text == _rankText.text)
                    .All(chapter => chapter.AllQuestionsCompleted()))
                {
                    LocalDataSystem.SetRank(_rankText.text.NextRank());
                    UpdateState();
                }
                return;
            }
        }

        public void Setup(string subject, List<Question> questions, string rank)
        {
            _questions = questions;
            _title.text = subject;
            _rankText.text = rank;
            UpdateState();
            PopupHolder.currentPopupType = PopupType.SubjectChapter;
            LevelLoader.Instance.questionChapters.Clear();
            CheckRankUp();
        }

        private void UpdateState()
        {
            SetAvailable(LocalDataSystem.localData.userInfo.rank.IsEqualRankOrGreaterThan(_rankText.text));
        }

        private void SetAvailable(bool available)
        {
            if (AllQuestionsCompleted())
            {
                _image.sprite = _completedSprite;
                _button.interactable = true;
                return;
            }
            
            _image.sprite = available ? _availableSprite : _lockedSprite;
            _button.interactable = available;
        }

        public bool AllQuestionsCompleted()
        {
            return _questions.Count > 0 && _questions.All(question => LocalDataSystem.IsQuestionCompleted(question.title));
        }
        
        private void OnCloseChapter()
        {
            CloseButton.instance.onClick -= OnCloseChapter;
        }

        public void OnClick()
        {
            foreach (var question in _questions)
            {
                var instance = Instantiate(LevelLoader.Instance.questionChapterPrefab, LevelLoader.Instance.rankViewContent);
                instance.Setup(question, this, CheckRankUp);

                LevelLoader.Instance.questionChapters.Add(instance);
            }
            
            SubjectChaptersClose();

        }

        private void SubjectChaptersClose()
        {
            //_subjectChapters.ForEach(subjectChapter => subjectChapter.gameObject.SetActive(false));
            LevelLoader.Instance.subjectChapters.ForEach(subjectChapter => subjectChapter.gameObject.SetActive(false));
        }
    }
}