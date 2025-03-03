using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.User
{
    public class UserProvider : MonoBehaviour
    {
        public TMP_Text _rankText;
        
        private void Awake()
        {
            UpdateState();
            LocalDataSystem.OnRankUp += OnRankUp;
        }

        private void OnDestroy()
        {
            LocalDataSystem.OnRankUp -= OnRankUp;
        }
        
        private void OnRankUp()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            _rankText.text = LocalDataSystem.localData.userInfo.rank.Replace("Plus", "+");
        }
    }
}