using System;
using Plankton.Proxy;
using UnityEngine;

namespace Plankton
{
    public class Ad
    {
        private const string logName = "[Ad]";

        private static ProxyAd proxyAd = null;
        private static string admobTestDeviceIds = string.Empty;

        public static bool IsInitialized { get; private set; } = false;
        public static bool IsOnline => proxyAd == null || proxyAd.IsOnline();

        public static event Action<AdMobImpressionRevenue> OnAdMobImpressionRevenue = null;
        public static event Action<AppLovinMaxImpressionRevenue> OnAppLovinMaxImpressionRevenue = null;

        private static bool IsNotInitialized
        {
            get
            {
                if (proxyAd == null)
                    Debug.Log($"{logName} Feature needs to be initialized first!");
                return proxyAd == null;
            }
        }

        public static void SetAdmobTestDeviceIds(string deviceIds)
        {
            admobTestDeviceIds = deviceIds;
        }

        public static void Initialize(Provider bannerProvider, string bannerZoneId, Provider interstitialProvider, string interstitialZoneId, Provider rewardedProvider, string rewardedZoneId, Action callback)
        {
            if (proxyAd != null)
            {
                Debug.Log($"{logName} Feature already initialized!");
                return;
            }

            PlanktonMono.onAdInitialized = () =>
            {
                IsInitialized = true;
                callback?.Invoke();
            };

            PlanktonMono.OnAdMobImpressionRevenueEvent += json =>
            {
                try
                {
                    var result = JsonUtility.FromJson<AdMobImpressionRevenue>(json);
                    OnAdMobImpressionRevenue?.Invoke(result);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            };

            PlanktonMono.OnAppLovinMaxImpressionRevenueEvent += json =>
            {
                try
                {
                    var result = JsonUtility.FromJson<AppLovinMaxImpressionRevenue>(json);
                    OnAppLovinMaxImpressionRevenue?.Invoke(result);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            };

            Utils.Jsoner.AddParams("admobTestDeviceIds", admobTestDeviceIds);
            var extras = Utils.Jsoner.GetJsonAndClear();
            proxyAd = new ProxyAd(GetProviderName(bannerProvider), bannerZoneId, GetProviderName(interstitialProvider), interstitialZoneId, GetProviderName(rewardedProvider), rewardedZoneId, extras);

#if UNITY_EDITOR
            PlanktonMono.CallInUnityThread(PlanktonMono.onAdInitialized);
#endif
        }


        public static class Banner
        {
            public static bool blackBackground = true;

            /// <summary>
            /// Request to show banner at the top of the screen.
            /// </summary>
            /// <param name="placement">The placement would be used in analytics</param>
            public static void ShowAtTop(string placement = "top")
            {
                if (IsNotInitialized) return;
                proxyAd?.Show(Type.BannerAtTop, placement);
                PlanktonMono.bannerBox = blackBackground ? 2 : 0;
            }

            /// <summary>
            /// Request to show banner at the bottom of the screen.
            /// </summary>
            /// <param name="placement">The placement would be used in analytics</param>
            public static void ShowAtBottom(string placement = "bottom")
            {
                if (IsNotInitialized) return;
                proxyAd?.Show(Type.BannerAtBottom, placement);
                PlanktonMono.bannerBox = blackBackground ? 1 : 0;
            }

            /// <summary>
            /// Hide any banner from the screen
            /// </summary>
            public static void Hide()
            {
                proxyAd?.Hide("banner");
                PlanktonMono.bannerBox = 0;
            }
        }

        public static class Rewarded
        {
            public static bool blackBackground = false;

            /// <summary>
            /// True if Rewarded Ad has been loaded and ready to show
            /// </summary>
            public static bool IsLoaded =>
#if UNITY_EDITOR
                proxyAd != null;
#else
                IsInitialized && proxyAd.IsReady(Type.Rewarded);
#endif

            /// <summary>
            /// try to show rewarded ad.
            /// </summary>
            /// <param name="onResult">Result callback (displayed, rewarded)</param>
            /// <param name="placement">The placement would be used in analytics</param>
            public static void Show(Action<bool, bool> onResult = null, string placement = "rewarded")
            {
                if (IsNotInitialized)
                {
                    onResult?.Invoke(false, false);
                    return;
                }

                var timescale = Time.timeScale;
                PlanktonMono.onAdClosed = json =>
                {
                    Time.timeScale = timescale;
                    AudioListenerSetPause(false);
                    PlanktonMono.fullscreenBox = false;

                    var result = Utils.Jsoner.FromJson(json, new AdClosedResult());
                    onResult?.Invoke(result.displayed.ToLower() == "true", result.reward_earned.ToLower() == "true");
                };

                Time.timeScale = 0;
                AudioListenerSetPause(true);
                PlanktonMono.fullscreenBox = blackBackground;
                proxyAd?.Show(Type.Rewarded, placement);

#if UNITY_EDITOR
                PlanktonMono.editorFullscreenAdCaption = $"{Type.Rewarded}\n{placement}";
                PlanktonMono.editorFullscreenAdTimer = 3;
#endif
            }
        }

        public static class Interstitial
        {
            public static bool blackBackground = false;

            /// <summary>
            /// True if Interstitial Ad has been loaded and ready to show
            /// </summary>
            public static bool IsLoaded =>
#if UNITY_EDITOR
                proxyAd != null;
#else
                IsInitialized && proxyAd.IsReady(Type.Interstitial);
#endif

            /// <summary>
            /// try to show Interstitial ad.
            /// </summary>
            /// <param name="onResult">Result callback (displayed)</param>
            /// <param name="placement">The placement would be used in analytics</param>
            public static void Show(Action<bool> onResult = null, string placement = "interstitial")
            {
                if (IsNotInitialized)
                {
                    onResult?.Invoke(false);
                    return;
                }

                var timescale = Time.timeScale;
                PlanktonMono.onAdClosed = json =>
                {
                    Time.timeScale = timescale;
                    AudioListenerSetPause(false);
                    PlanktonMono.fullscreenBox = false;

                    var result = Utils.Jsoner.FromJson(json, new AdClosedResult());
                    onResult?.Invoke(result.displayed.ToLower() == "true");
                };

                Time.timeScale = 0;
                AudioListenerSetPause(true);
                PlanktonMono.fullscreenBox = blackBackground;
                proxyAd?.Show(Type.Interstitial, placement);

#if UNITY_EDITOR
                PlanktonMono.editorFullscreenAdCaption = $"{Type.Interstitial}\n{placement}";
                PlanktonMono.editorFullscreenAdTimer = 3;
#endif
            }
        }

        private static void AudioListenerSetPause(bool value)
        {
            AudioListener.pause = value;
        }

        public static void OpenAdMobDebugger()
        {
            if (IsNotInitialized) return;
            proxyAd.TestAdMob();
        }

        public static void OpenAppLovinMaxDebugger()
        {
            if (IsNotInitialized) return;
            proxyAd.TestMax();
        }
        
        public static void OpenLevelPlayDebugger()
        {
            if (IsNotInitialized) return;
            proxyAd.TestLevelPlay();
        }

        public static string GetAndroidAdId()
        {
#if UNITY_ANDROID
            if (IsNotInitialized) return string.Empty;
            return proxyAd.GetAndroidAdId();
#else
            return string.Empty;
#endif
        }

        private static string GetProviderName(Provider provider)
        {
            switch (provider)
            {
                case Provider.Admob: return "admob";
                case Provider.AppLovin: return "applovin";
                case Provider.LevelPlay: return "levelplay";
#if TAPSELL
                case Provider.Tapsell: return "tapsellplus";
#endif
                default: return string.Empty;
            }
        }

        //////////////////////////////////////////////////////
        /// HELPER CLASSES
        //////////////////////////////////////////////////////
        public enum Provider
        {
            Admob,
            AppLovin,
            LevelPlay,
#if TAPSELL
            Tapsell
#endif
        }

        private static class Type
        {
            public const string Interstitial = "interstitial";
            public const string Rewarded = "rewarded";
            public const string BannerAtTop = "banner_top";
            public const string BannerAtBottom = "banner_bottom";
        }

        [Serializable]
        private class AdClosedResult
        {
            public string displayed;
            public string reward_earned;
        }

        [Serializable]
        public class AdMobImpressionRevenue
        {
            public string adUnitId;
            public string adType;
            public string currencyCode;
            public AdMobImpressionPrecision precision;
            public string networkName;
            public long revenue;
            public string adMobSdkVersion;
            public string responseId;
        }

        [Serializable]
        public class AppLovinMaxImpressionRevenue
        {
            public string adUnitId;
            public string adType;
            public string networkName;
            public string country;
            public string placement;
            public string creativeId;
            public double revenue;
            public string appLovinSdkVersion;
            public string precision;
        }
    }
}