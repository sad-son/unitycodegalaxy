using System;
using System.Collections.Generic;
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

        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void Setup(string subject, List<Question> questions, string rank)
        {
            _questions = questions;
            _title.text = subject;
            _rankText.text = rank;
            PopupHolder.currentPopupType = PopupType.SubjectChapter;
            LevelLoader.Instance.questionChapters.Clear();
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
                instance.Setup(question, this);

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