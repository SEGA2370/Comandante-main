using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    public static AdsManager I { get; private set; }

    [Header("Ad Unit IDs (TEST)")]
    [SerializeField] string androidInterstitialId = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField] string androidRewardedId = "ca-app-pub-3940256099942544/5224354917";

    InterstitialAd interstitial;
    RewardedAd rewarded;

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        // Всегда инициализируем AdMob – revive нужен даже после Remove Ads
        MobileAds.Initialize(_ =>
        {
            PreloadInterstitial();
            PreloadRewarded();
        });

        AdsPolicy.OnAdsRemovedChanged += OnAdsRemoved;
    }


    private void OnDestroy()
    {
        AdsPolicy.OnAdsRemovedChanged -= OnAdsRemoved;
    }

    private void OnAdsRemoved()
    {
        // optional: clean up loaded ads
        interstitial?.Destroy();
        interstitial = null;
    }

    #region Interstitial (already used by restart-after-death)
    public void PreloadInterstitial()
    {
        InterstitialAd.Load(androidInterstitialId, new AdRequest(), (ad, err) =>
        {
            if (err != null) { Debug.LogWarning($"[AdMob] Interstitial load error: {err.GetMessage()}"); return; }
            interstitial = ad;
            interstitial.OnAdFullScreenContentClosed += () =>
            {
                interstitial?.Destroy();
                interstitial = null;
                PreloadInterstitial();
            };
            interstitial.OnAdFullScreenContentFailed += _ =>
            {
                interstitial?.Destroy();
                interstitial = null;
                PreloadInterstitial();
            };
        });
    }

    public void ShowInterstitial(Action onClosed)
    {
        if (AdsPolicy.AdsRemoved) { onClosed?.Invoke(); return; }

        if (interstitial != null)
        {
            interstitial.Show();
            interstitial.OnAdFullScreenContentClosed += () => onClosed?.Invoke();
            interstitial.OnAdFullScreenContentFailed += _ => onClosed?.Invoke();
        }
        else
        {
            onClosed?.Invoke();
            PreloadInterstitial();
        }
    }
    #endregion

    #region Rewarded
    public void PreloadRewarded()
    {
        // НЕ проверяем AdsPolicy.AdsRemoved – revive всегда доступен
        RewardedAd.Load(androidRewardedId, new AdRequest(), (ad, err) =>
        {
            if (err != null) { Debug.LogWarning($"[AdMob] Rewarded load error: {err.GetMessage()}"); return; }
            rewarded = ad;

            rewarded.OnAdFullScreenContentClosed += () =>
            {
                rewarded?.Destroy();
                rewarded = null;
                PreloadRewarded();
            };
            rewarded.OnAdFullScreenContentFailed += _ =>
            {
                rewarded?.Destroy();
                rewarded = null;
                PreloadRewarded();
            };
        });
    }

    /// onResult(true) if reward granted; if ads are removed and NOT forced → auto-grant.
    public void ShowRewarded(Action<bool> onResult, bool forceEvenIfRemoved = false)
    {
        if (AdsPolicy.AdsRemoved && !forceEvenIfRemoved)
        {
            onResult?.Invoke(true);   // skip ad, treat as granted
            return;
        }

        if (rewarded == null)
        {
            onResult?.Invoke(false);
            PreloadRewarded();
            return;
        }

        bool granted = false;
        rewarded.Show((Reward r) => { granted = true; });
        rewarded.OnAdFullScreenContentClosed += () => onResult?.Invoke(granted);
        rewarded.OnAdFullScreenContentFailed += _ => onResult?.Invoke(false);
    }
    #endregion
}
