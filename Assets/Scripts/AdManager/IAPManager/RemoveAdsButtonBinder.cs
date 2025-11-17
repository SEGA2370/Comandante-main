using UnityEngine;

public class RemoveAdsButtonBinder : MonoBehaviour
{
    void OnEnable()
    {
        AdsPolicy.OnAdsRemovedChanged += Refresh;
        Refresh();
    }

    void OnDisable()
    {
        AdsPolicy.OnAdsRemovedChanged -= Refresh;
    }

    public void Refresh()
    {
        gameObject.SetActive(!AdsPolicy.AdsRemoved);
    }
}
