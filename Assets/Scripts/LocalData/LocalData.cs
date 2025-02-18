using System;
using DefaultNamespace.User;

namespace DefaultNamespace
{
    [Serializable]
    public class LocalData
    {
        public UserInfo userInfo = new();
        public QuizDataLocal quizData = new();
    }
}