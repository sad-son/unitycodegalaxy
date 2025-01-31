using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum Rank
{
    MegaJunior = 1,
    Junior = 2,
    
    God = 3
}

[Serializable]
public class Answer
{
    public string answer;
    public bool right;
}

[Serializable]
public struct Question
{
    public string title;
    public Answer[] answers;
}

[Serializable]
public class QuizData
{
    public Question[] questions;
}

public class Quiz : MonoBehaviour
{
    public QuizItem quizItem;
    public TMP_Text levelText;
    public TMP_Text rankText;
    public TMP_Text titleText;
    public RectTransform questionsPanel;

    public Action onCompleted;

    public List<QuizItem> quizItems = new List<QuizItem>();
    
    private void OnDestroy()
    {
        onCompleted = null;
    }

    public void Setup(Question question)
    {
        levelText.text = $"current level: {LevelLoader.GetLevel()}";
        var rankIndex = (int)LevelLoader.GetLevel() / 1;
        var rank = rankIndex >= Enum.GetNames(typeof(Rank)).Length ? Rank.God.ToString() : Enum.GetName(typeof(Rank), rankIndex);
        rankText.text = $"rank: {rank}";
        
        titleText.text = question.title;
        Shuffle(question.answers);
        var first = !quizItems.Any();
        
        for (var i = 0; i < question.answers.Length; i++)
        {
            var answer = question.answers[i];
            
            if (first)
            {
                var instance = Instantiate(quizItem, questionsPanel);
                quizItems.Add(instance);
                instance.Setup(answer, Complete);
                continue;
            }
           
            quizItems[i].Setup(answer, Complete);
        }
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
