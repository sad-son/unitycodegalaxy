using System;
using System.Collections.Generic;

[Serializable]
public class QuizData
{
    public string rank;
    public Dictionary<string, List<Question>> subjects;
}