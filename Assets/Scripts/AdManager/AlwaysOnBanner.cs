using UnityEngine;
using GoogleMobileAds.Api;

public class AlwaysOnBanner : MonoBehaviour
{
    [Header("Test banner (Android)")]
    [SerializeField] string androidBannerId = "ca-app-pub-3940256099942544/6300978111";

    [Header("Optional UI Spacer (to avoid overlap)")]
    [SerializeField] RectTransform uiBottomSpacer; // drag a bottom panel from your Canvas if you want padding

    BannerView banner;
    int lastW, lastH;

    private static AlwaysOnBanner _instance;

    private void Awake()
    {
        if (_instance != null) { Destroy(gameObject); return; }
        _instance = this;

        // hide if user bought remove ads
        if (AdsPolicy.AdsRemoved) { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
        MobileAds.Initialize(_ => CreateOrReload());

        AdsPolicy.OnAdsRemovedChanged += OnAdsRemoved;
    }

    void OnDestroy()
    {
        AdsPolicy.OnAdsRemovedChanged -= OnAdsRemoved;
        banner?.Destroy();
    }

    void OnAdsRemoved()
    {
        banner?.Destroy();
        Destroy(gameObject);
    }

    void Update()
    {
        if (Screen.width != lastW || Screen.height != lastH)
        {
            lastW = Screen.width; lastH = Screen.height;
            CreateOrReload();
        }
    }

    void CreateOrReload()
    {
        banner?.Destroy();

        // Addaptive size for current orientation
        int widthPx = Mathf.Clamp(Screen.width, 320, 2048);
        AdSize size = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(widthPx);

#if UNITY_ANDROID 
        string adUnitId = androidBannerId;
#else
        string adUnitID = androidBannerId;
#endif
        banner = new BannerView(adUnitId, size, AdPosition.Bottom);
        banner.LoadAd(new AdRequest());

        banner.OnBannerAdLoaded += () =>
        {
            //Apply padding so your HUD doesn't sit under banner (optional)
            if (uiBottomSpacer != null)
            {
                var canvas = uiBottomSpacer.GetComponentInParent<Canvas>();
                float scale = (canvas != null && canvas.renderMode != RenderMode.WorldSpace) ? canvas.scaleFactor : 1f;
                float hPx = banner.GetHeightInPixels();
                var sd = uiBottomSpacer.sizeDelta;
                sd.y = hPx / Mathf.Max(0, 01f, scale);
                uiBottomSpacer.sizeDelta = sd;
            }
        };
        banner.OnBannerAdLoadFailed += err => Debug.LogWarning($"[AdMob] Banner failed: {err.GetMessage()}");
    }
}
