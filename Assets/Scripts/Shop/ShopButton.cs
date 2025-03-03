using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ShopButton : MonoBehaviour
    {
        [SerializeField] private Shop shop;
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void OnClick()
        {
            shop.gameObject.SetActive(true);
        }
    }
}