using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class CloseButton : MonoBehaviour
    {
        public static CloseButton instance;
        public event Action onClick;
        
        public Button _button;
        private LevelLoader _levelLoader;
        
        private void Awake()
        {
            instance = this;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void Start()
        {
            _levelLoader = LevelLoader.instance;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
            onClick = null;
        }

        private void OnClick()
        {
            if (PopupHolder.currentPopupType == PopupType.QuestionChapter)
            {
                CloseQuestionChapter();
                return;
            }
            
            if (PopupHolder.currentPopupType == PopupType.SubjectChapter)
            {
                PopupHolder.currentPopupType = PopupType.RankChapter;
                _levelLoader.rankChapters.ForEach(chapter => chapter.gameObject.SetActive(true));
                _levelLoader.subjectChapters.ForEach(chapter => Destroy(chapter.gameObject));
                _levelLoader.subjectChapters.Clear();
                return;
            }
            
            onClick?.Invoke();
        }

        public void CloseQuestionChapter()
        {
            PopupHolder.currentPopupType = PopupType.SubjectChapter;
            _levelLoader.subjectChapters.ForEach(subjectChapter => subjectChapter.gameObject.SetActive(true));
            _levelLoader.questionChapters.ForEach(chapter => Destroy(chapter.gameObject));
            _levelLoader.questionChapters.Clear();
        }
    }
}