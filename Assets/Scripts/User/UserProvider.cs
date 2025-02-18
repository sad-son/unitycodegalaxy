using System;
using TMPro;
using UnityEngine;

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
            Debug.LogError($"SAD current rank {LocalDataSystem.localData.userInfo.rank}");
            _rankText.text = LocalDataSystem.localData.userInfo.rank;
        }
    }
}