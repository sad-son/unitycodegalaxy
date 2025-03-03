using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Shop : MonoBehaviour
    {
        public static Shop Instance;

        public Button purchaseButton;
        public Button noAdsButton;
        public Button infiniteLivesButton;
        public Sprite _selectedSprite;
        public Sprite _defaultSprite;

        private string _selectedPurchaseId;
        private void Awake()
        {
            Instance = this;
            
            infiniteLivesButton.onClick.AddListener(OnClickInfiniteLivesButton);
            noAdsButton.onClick.AddListener(OnClickNoAdsButton);
            purchaseButton.onClick.AddListener(OnClickPurchaseButton);
        }

        private void OnClickPurchaseButton()
        {
            if (!string.IsNullOrEmpty(_selectedPurchaseId))
                Debug.Log($"OnClickPurchaseButton {_selectedPurchaseId}");
        }

        private void OnDestroy()
        {
            infiniteLivesButton.onClick.RemoveListener(OnClickInfiniteLivesButton);
            noAdsButton.onClick.RemoveListener(OnClickNoAdsButton);
            purchaseButton.onClick.RemoveListener(OnClickPurchaseButton);
        }

        private void OnClickNoAdsButton()
        {
            infiniteLivesButton.image.sprite = _defaultSprite;
            noAdsButton.image.sprite = _selectedSprite;
            _selectedPurchaseId = "NoAds";
        }

        private void OnClickInfiniteLivesButton()
        {
            infiniteLivesButton.image.sprite = _selectedSprite;
            noAdsButton.image.sprite = _defaultSprite;
            _selectedPurchaseId = "InfiniteLives";
        }
    }
}