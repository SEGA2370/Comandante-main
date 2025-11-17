using UnityEngine;
using UnityEngine.UI;

public class RemoveAdsButtonController : MonoBehaviour
{
    [SerializeField] Button buyButton;   // drag this same Button here
    [SerializeField] GameObject spinner; // optional: show while processing

    void Awake()
    {
        if (!buyButton) buyButton = GetComponent<Button>();
    }

    public void OnClickBuy()
    {
        if (AdsPolicy.AdsRemoved) return;
        SetBusy(true);

        if (IAPManager.I == null)
        {
            Debug.LogWarning("[UI] No IAPManager in scene");
            SetBusy(false);
            return;
        }

        IAPManager.I.BuyRemoveAds(
            onSuccess: () =>
            {
                SetBusy(false);
                // ”спешно купили Ц AdsPolicy уже обновилс€ внутри GrantRemoveAds()
            },
            onFailed: () =>
            {
                SetBusy(false);
                Debug.LogWarning("[UI] Remove Ads purchase failed or cancelled");
            });
    }


    void SetBusy(bool busy)
    {
        if (buyButton) buyButton.interactable = !busy;
        if (spinner) spinner.SetActive(busy);
    }
}
