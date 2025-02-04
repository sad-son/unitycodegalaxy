using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class RankChapter : MonoBehaviour
    {
        public TMP_Text text;

        private QuizData _quizData;
        private LevelLoader _levelLoader;
        private Button _button;
        private SubjectChapter _subjectChapterPrefab;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void Setup(LevelLoader levelLoader, QuizData quizData, SubjectChapter subjectChapter)
        {
            _levelLoader = levelLoader;
            _subjectChapterPrefab = subjectChapter;
            _quizData = quizData;
            text.text = quizData.rank;
        }

        public void OnClick()
        {
            foreach (var subject in _quizData.subjects)
            {
                Instantiate(_subjectChapterPrefab, _levelLoader.canvas).Setup(subject.Key);
            }
        }
    }
}