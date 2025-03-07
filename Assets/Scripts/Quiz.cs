using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Quiz : MonoBehaviour
{
    public static Quiz instance;
    public QuizItem quizItem;
    public TMP_Text titleText;
    public RectTransform questionsPanel;

    public Action onCompleted;

    public List<QuizItem> quizItems = new List<QuizItem>();
    private List<GameObject> _childs = new List<GameObject>();
    private void Awake()
    {
        instance = this;
        _childs = transform.GetComponentsInChildren<Transform>(true).Select(x => x.gameObject).ToList();
        SetActive(false);
    }

    private void OnDestroy()
    {
        onCompleted = null;
    }

    public void SetActive(bool active)
    {
        _childs.ForEach(x =>
        {
            if (x != gameObject)
                x.SetActive(active);
        });
    }
    
    public void Setup(Question question)
    {
        SetActive(true);
        CloseButton.instance.gameObject.SetActive(false);
        titleText.text = question.title;
        Shuffle(question.answers);
        var quizCreated = !quizItems.Any();
        
        for (var i = 0; i < question.answers.Length; i++)
        {
            var answer = question.answers[i];
            
            if (quizCreated)
            {
                var instance = Instantiate(quizItem, questionsPanel);
                quizItems.Add(instance);
                instance.Setup(answer, Complete);
                continue;
            }
           
            quizItems[i].Setup(answer, Complete);
        }
        
        PopupHolder.currentPopupType = PopupType.Quiz;
    }

    
    public static void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1); 
            (array[i], array[randomIndex]) = (array[randomIndex], array[i]); 
        }
    }
    
    public void Complete()
    {
        onCompleted?.Invoke();
    }
}
