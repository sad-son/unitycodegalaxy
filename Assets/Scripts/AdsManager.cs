using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Unity.Services.LevelPlay;
using UnityEngine.Advertisements;

namespace DefaultNamespace
{
    public class AdsManager : MonoBehaviour
    {
        [SerializeField] string androidGameID = "5805203";
        [SerializeField] string iOSGameID = "5805202";
        [SerializeField] bool testMode = true;
        [SerializeField] string androidAdID = "Interstitial_Android";
        [SerializeField] string iOSAdID = "Interstitial_iOS";
        [SerializeField] BannerAd unityBannerAd;
        
        public static AdsManager instance;
        private LevelPlayInterstitialAd interstitialAd;
        
        private string gameID;
        private string adID;
#if UNITY_ANDROID
        string appKey = "213254fc5"; //iron-source
       // string appKey = "19b5509a-79f2-4586-b92d-0df96a71ac10"; //unity
#elif UNITY_IPHONE
    string appKey = "8545d445";
#else
    string appKey = "unexpected_platform";
#endif
        
        public void LoadInterstitial()
        {
            InterstitialAd.instance.ShowAd();
        }

        private void Awake()
        {
            instance = this;
        }

        public void Load()
        {
            gameID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSGameID : androidGameID;
            adID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSAdID : androidAdID;
            Debug.Log("unity-script: IronSource.Agent.validateIntegration");

            IronSourceConfig.Instance.setClientSideCallbacks(true);

            var id = IronSource.Agent.getAdvertiserId();
            Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + id);
            
            IronSource.Agent.validateIntegration();
            IronSource.Agent.setConsent(true);
            // SDK init
            Debug.Log("unity-script: LevelPlay SDK initialization");
            com.unity3d.mediation.LevelPlayAdFormat[] legacyAdFormats = new[]
            {
                com.unity3d.mediation.LevelPlayAdFormat.INTERSTITIAL,
                com.unity3d.mediation.LevelPlayAdFormat.BANNER,
                
            };
            IronSource.Agent.init(appKey, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);
            LevelPlay.Init(appKey, adFormats: legacyAdFormats);
            LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
            LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
           
        }

        void EnableAds()
        {
            //Add ImpressionSuccess Event
            IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;

            //Add AdInfo Rewarded Video Events
            IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
            InterstitialAd.instance.Initialize();
            BannerAd.instance.LoadBanner();
        }

        void OnApplicationPause(bool isPaused)
        {
            Debug.Log("unity-script: OnApplicationPause = " + isPaused);
            IronSource.Agent.onApplicationPause(isPaused);
        }

        #region Init callback handlers

        void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
        {
            Debug.Log("unity-script: I got SdkInitializationCompletedEvent with config: " + config);
            EnableAds();
        }

        void SdkInitializationFailedEvent(LevelPlayInitError error)
        {
            Debug.Log("unity-script: I got SdkInitializationFailedEvent with error: " + error);
        }

        #endregion

        #region AdInfo Rewarded Video

        void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got RewardedVideoOnAdOpenedEvent With AdInfo " + adInfo);
        }

        void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got RewardedVideoOnAdClosedEvent With AdInfo " + adInfo);
        }

        void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got RewardedVideoOnAdAvailable With AdInfo " + adInfo);
        }

        void RewardedVideoOnAdUnavailable()
        {
            Debug.Log("unity-script: I got RewardedVideoOnAdUnavailable");
        }

        void RewardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got RewardedVideoOnAdShowFailedEvent With Error" + ironSourceError +
                      "And AdInfo " + adInfo);
        }

        void RewardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got RewardedVideoOnAdRewardedEvent With Placement" + ironSourcePlacement +
                      "And AdInfo " + adInfo);
        }

        void RewardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got RewardedVideoOnAdClickedEvent With Placement" + ironSourcePlacement +
                      "And AdInfo " + adInfo);
        }

        #endregion

        #region AdInfo Interstitial

        void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got InterstitialOnAdLoadedEvent With AdInfo " + adInfo);
            if (interstitialAd.IsAdReady())
            {
                interstitialAd.ShowAd();
            }
            else
            {
                Debug.Log("unity-script: Levelplay Interstital Ad Ready? - False");
            }
        }

        void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
        {
            Debug.Log("unity-script: I got InterstitialOnAdLoadFailedEvent With Error " + error);
        }

        void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got InterstitialOnAdDisplayedEvent With AdInfo " + adInfo);
        }

        void InterstitialOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError infoError)
        {
            Debug.Log("unity-script: I got InterstitialOnAdDisplayFailedEvent With InfoError " + infoError);
        }

        void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got InterstitialOnAdClickedEvent With AdInfo " + adInfo);
        }

        void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got InterstitialOnAdClosedEvent With AdInfo " + adInfo);
        }

        void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got InterstitialOnAdInfoChangedEvent With AdInfo " + adInfo);
        }

        #endregion

        #region Banner AdInfo

        void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got BannerOnAdLoadedEvent With AdInfo " + adInfo);
        }

        void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError)
        {
            Debug.Log("unity-script: I got BannerOnAdLoadFailedEvent With Error " + ironSourceError);
        }

        void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got BannerOnAdClickedEvent With AdInfo " + adInfo);
        }

        void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got BannerOnAdDisplayedEvent With AdInfo " + adInfo);
        }

        void BannerOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError adInfoError)
        {
            Debug.Log("unity-script: I got BannerOnAdDisplayFailedEvent With AdInfoError " + adInfoError);
        }

        void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got BannerOnAdCollapsedEvent With AdInfo " + adInfo);
        }

        void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got BannerOnAdLeftApplicationEvent With AdInfo " + adInfo);
        }

        void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got BannerOnAdExpandedEvent With AdInfo " + adInfo);
        }

        #endregion

        #region ImpressionSuccess callback handler

        void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
        {
            Debug.Log("unity - script: I got ImpressionDataReadyEvent ToString(): " + impressionData.ToString());
            Debug.Log("unity - script: I got ImpressionDataReadyEvent allData: " + impressionData.allData);
        }

        #endregion

        private void OnDisable()
        {
            interstitialAd?.DestroyAd();
        }
    }
}