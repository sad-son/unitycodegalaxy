using System;
using UnityEngine;
using Unity.Services.LevelPlay;

public class BannerAd : MonoBehaviour
{
    private string adUnitId = "ubrowrk1k4t7jt05"; // Placement ID из Unity Dashboard
    private string placementName = "Banner_Android"; // Placement ID из Unity Dashboard
    private LevelPlayBannerAd bannerAd;
    public static BannerAd instance;
    private void Awake()
    {
        instance = this;
    }
    
    public void LoadBanner()
    {
        Debug.Log("Загрузка баннера через LevelPlay");
  
        bannerAd = new LevelPlayBannerAd(adUnitId, size:com.unity3d.mediation.LevelPlayAdSize.BANNER,
            placementName: placementName);
        // Register to the events 
        bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;
        bannerAd.LoadAd();
        ShowBanner();

    }

    public void ShowBanner()
    {
        bannerAd.ShowAd();
    }

    private void OnDestroy()
    {
        bannerAd?.DestroyAd();
    }

    void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("BannerOnAdLoadedEvent");
    }

    void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError)
    {
        Debug.Log($"BannerOnAdLoadFailedEvent {ironSourceError}");
    }

    void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("BannerOnAdClickedEvent");
    }

    void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("BannerOnAdDisplayedEvent");
    }

    void BannerOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError adInfoError)
    {
        Debug.Log($"BannerOnAdDisplayFailedEvent {adInfoError}");
    }

    void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("BannerOnAdCollapsedEvent");
    }

    void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("BannerOnAdLeftApplicationEvent");
    }

    void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("BannerOnAdExpandedEvent");
    }
}