using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class SubjectChapter : MonoBehaviour
    {
        public Dictionary<string, List<Question>> _subjects;
        public TMP_Text _text;
        private List<SubjectChapter> _subjectChapters = new();
        public List<QuestionChapter> _questionChapters = new();
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

        public void Setup(string subject, List<Question> questions, List<SubjectChapter> subjectChapters)
        {
            _questions = questions;
            _subjectChapters = subjectChapters;
            _text.text = subject;
        }
        
        public void OnClick()
        {
            _subjectChapters.ForEach(rankChapter => rankChapter.gameObject.SetActive(false));
            
            foreach (var question in _questions)
            {
                var instance = Instantiate(LevelLoader.Instance.questionChapterPrefab, LevelLoader.Instance.rankViewContent);
                instance.Setup(question, this);

                _questionChapters.Add(instance);
            }
        }
    }
}