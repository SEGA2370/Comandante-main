using UnityEngine;

public class IAPManager : MonoBehaviour
{
    public static IAPManager I { get; private set; }

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    // Call this after a successful purchase via Unity IAP
    public void GrantRemoveAds()
    {
        AdsPolicy.SetAdsRemoved(true);

        // Kill any active banner immediately
        var banner = FindAnyObjectByType<AlwaysOnBanner>();
        if (banner != null) Destroy(banner.gameObject);
        Debug.Log("[IAP] Remove Ads granted.");
    }
}
