using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager I { get; private set; }

    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

    // TODO: подставь сюда свой реальный productId из Google Play
    public const string REMOVE_ADS_PRODUCT_ID = "remove_ads";

    // callbacks дл€ UI
    private Action _onBuySuccess;
    private Action _onBuyFailed;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (storeController == null)
            InitializePurchasing();
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // наш non-consumable Remove Ads
        builder.AddProduct(REMOVE_ADS_PRODUCT_ID, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    // UI вызывает это дл€ покупки Remove Ads
    public void BuyRemoveAds(Action onSuccess, Action onFailed)
    {
        _onBuySuccess = onSuccess;
        _onBuyFailed = onFailed;
        BuyProductID(REMOVE_ADS_PRODUCT_ID);
    }

    private void BuyProductID(string productId)
    {
        if (!IsInitialized())
        {
            Debug.LogWarning("[IAP] Not initialized yet");
            _onBuyFailed?.Invoke();
            return;
        }

        Product product = storeController.products.WithID(productId);

        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"[IAP] Purchasing product: {productId}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.LogWarning("[IAP] BuyProductID: product not found or not available");
            _onBuyFailed?.Invoke();
        }
    }

    // --- IStoreListener ---

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("[IAP] Initialized");
        storeController = controller;
        storeExtensionProvider = extensions;

        // ≈сли игрок уже покупал Remove Ads раньше Ц восстановим
        Product removeAds = storeController.products.WithID(REMOVE_ADS_PRODUCT_ID);
        if (removeAds != null && removeAds.hasReceipt)
        {
            Debug.Log("[IAP] Remove Ads already owned, applying");
            GrantRemoveAds();
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogWarning("[IAP] InitializeFailed: " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogWarning($"[IAP] InitializeFailed: {error} Ц {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log("[IAP] ProcessPurchase: " + args.purchasedProduct.definition.id);

        if (String.Equals(args.purchasedProduct.definition.id, REMOVE_ADS_PRODUCT_ID, StringComparison.Ordinal))
        {
            Debug.Log("[IAP] Remove Ads purchase SUCCESS");
            GrantRemoveAds();
            _onBuySuccess?.Invoke();
        }
        else
        {
            Debug.LogWarning("[IAP] Unknown product: " + args.purchasedProduct.definition.id);
        }

        _onBuySuccess = null;
        _onBuyFailed = null;

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogWarning($"[IAP] Purchase failed: {product.definition.id}, {failureReason}");
        _onBuyFailed?.Invoke();
        _onBuySuccess = null;
        _onBuyFailed = null;
    }

    // --- тво€ логика выдачи Remove Ads ---

    public void GrantRemoveAds()
    {
        AdsPolicy.SetAdsRemoved(true);

        // ”биваем активный баннер, если есть
        var banner = UnityEngine.Object.FindAnyObjectByType<AlwaysOnBanner>();
        if (banner != null) UnityEngine.Object.Destroy(banner.gameObject);

        Debug.Log("[IAP] Remove Ads granted.");
    }
}
