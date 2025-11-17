using System;
using UnityEngine;

public static class AdsPolicy
{
    private const string PREFS_KEY = "ads_removed";

    public static bool AdsRemoved => PlayerPrefs.GetInt(PREFS_KEY, 0) == 1;
    public static event Action OnAdsRemovedChanged;

    public static void SetAdsRemoved(bool removed)
    {
        int v = removed ? 1 : 0;
        if (PlayerPrefs.GetInt(PREFS_KEY, 0) == v) return;

        PlayerPrefs.SetInt(PREFS_KEY, v);
        PlayerPrefs.Save();
        OnAdsRemovedChanged?.Invoke();
    }
}
