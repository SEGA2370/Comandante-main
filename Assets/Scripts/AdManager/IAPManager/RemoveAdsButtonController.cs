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

        // TODO: replace with real Unity IAP purchase call when product is set up:
        // IAPManager.I.BuyRemoveAds(productId);
        // For now we simulate success by granting immediately:
        IAPManager.I.GrantRemoveAds();

        SetBusy(false);
    }

    void SetBusy(bool busy)
    {
        if (buyButton) buyButton.interactable = !busy;
        if (spinner) spinner.SetActive(busy);
    }
}
