using System;

namespace DefaultNamespace.User
{
    [Serializable]
    public class UserInfo
    {
        public string name;
        public string rank = Rank.Junior.ToString();
    }
}