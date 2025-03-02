using Unity.Services.LevelPlay;
using UnityEngine;

namespace DefaultNamespace
{
    public class InterstitialAd : MonoBehaviour
    {
        private string adUnitId = "jo924pgb6ga79bj9"; // Placement ID из Unity Dashboard
        private LevelPlayInterstitialAd interstitialAd ;
        public static InterstitialAd instance;
        private void Awake()
        {
            instance = this;
        }
    
        public void Initialize()
        {
            Debug.Log("Загрузка InterstitialAd через LevelPlay");
  
            interstitialAd = new LevelPlayInterstitialAd(adUnitId);
            // Register to the events 
            interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
            interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
            interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
            interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
            interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
            interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
            interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;
        }

        public void ShowAd()
        {
            Debug.Log("Показ InterstitialAd через LevelPlay");
            interstitialAd?.LoadAd();
        }

        private void OnDestroy()
        {
            interstitialAd?.DestroyAd();
        }

        void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log($"InterstitialOnAdLoadedEvent {adInfo}");
            interstitialAd?.ShowAd("Interstitial_Android");
        }

        void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
        {
            Debug.Log($"InterstitialOnAdLoadFailedEvent {error}");
        }

        void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("InterstitialOnAdDisplayedEvent");
        }

        void InterstitialOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError infoError)
        {
            Debug.Log($"InterstitialOnAdDisplayFailedEvent {infoError}");
        }

        void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("InterstitialOnAdClickedEvent");
        }

        void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("InterstitialOnAdClosedEvent");
        }

        void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("InterstitialOnAdInfoChangedEvent");
        }
    }
}