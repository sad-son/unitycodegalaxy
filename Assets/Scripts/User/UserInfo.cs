using System;
using UnityEngine.Serialization;

namespace DefaultNamespace.User
{
    [Serializable]
    public class UserInfo
    {
        public string name;
        public string rank = Rank.Junior.ToString();
        public int health = 5;
        public int maxHealth = 5;
        public string nextLifeTime;
        [FormerlySerializedAs("infiniteLifes")] public bool infiniteLives;
    }
}