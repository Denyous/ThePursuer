using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using System.Threading.Tasks;


[Serializable]
public class NonConsumableItem
{
    public string Name;
    public string Id;
    public string Description;
    public float Price;
}

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    public NonConsumableItem NonConsumableItem;
    IStoreController storeController;

    public static IAPManager instance;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public async Task Initiate()
    {
        var options = new InitializationOptions()
                .SetEnvironmentName("Production");

        await UnityServices.InitializeAsync(options);

        SetupBuilder();
    }

    public void Purchase()
    {
        storeController.InitiatePurchase(NonConsumableItem.Id);
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;

        if(product.definition.id == NonConsumableItem.Id)
        {
            Debug.Log($"Purchase successful: {product.definition.id}");
            // Handle successful purchase logic here
            AdsManager.instance.RemoveAds();
            return PurchaseProcessingResult.Complete;
        }
        else
        {
            Debug.LogWarning($"Purchase failed: Unknown product {product.definition.id}");
            return PurchaseProcessingResult.Pending;
        }
    }

    private void SetupBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(NonConsumableItem.Id, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("INITIALIZED");
        storeController = controller;
        CheckNonConsumable(NonConsumableItem.Id);
    }

    private void CheckNonConsumable(string id)
    {
        //if (storeController != null)
        //{
        //    var product = storeController.products.WithID(id);

        //    if (product != null)
        //    {
        //        if (product.hasReceipt)
        //        {
        //            print($"Non-consumable {id} already purchased.");
        //            AdsManager.instance.RemoveAds();
        //        }
        //        else
        //        {
        //            print($"Non-consumable {id} is not purchased.");
        //        }
        //    }
        //    else
        //    {
        //        print($"Non-consumable {id} not found in store.");
        //    }
        //}
        //else
        //{
        //    print("Store controller is not initialized.");
        //}
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        print($"Purchase failed for product {product.definition.id} with reason: {failureDescription}");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print($"Initialization failed with error: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print($"Initialization failed with error: {message}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print($"Purchase failed for product {product.definition.id} with reason: {failureReason}");
    }
}
