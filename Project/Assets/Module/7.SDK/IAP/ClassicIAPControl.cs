using UnityEngine;
//using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Extension;
using System;

public class ClassicIAPControl : Singleton<ClassicIAPControl>//, IDetailedStoreListener
{

/*
    IStoreController m_StoreController; // The Unity Purchasing system.
    Action callbackSucceed;
    Action callbackFailed;

    public void Init()
    {
        InitializePurchasing();
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var iapData in GameData.allIap.dictIapData)
        {
            builder.AddProduct(iapData.Key, ProductType.Consumable);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public string OnGetLocalPriceString(string productID)
    {
        if(m_StoreController == null)
        {
            return "--";
        }
        float priceUSD = GameData.allIap.dictIapData[productID].priceUSD;
        return m_StoreController.products.WithID(productID).metadata.localizedPriceString;
        //return priceUSD.ToString();
    }

       public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");
            m_StoreController = controller;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            OnInitializeFailed(error, null);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

            if (message != null)
            {
                errorMessage += $" More details: {message}";
            }

            Debug.Log(errorMessage);
        }

        public void OnPurchaseConsumable(string productID, Action callbackSucceed, Action callbackFailed)
        {
            this.callbackSucceed = callbackSucceed;
            this.callbackFailed = callbackFailed;

            m_StoreController.InitiatePurchase(productID);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            //Retrieve the purchased product
            var product = args.purchasedProduct;

            Debug.Log($"Purchase Complete - Product: {product.definition.id}");
            callbackSucceed?.Invoke();
            //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
            callbackFailed?.Invoke();
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
                $" Purchase failure reason: {failureDescription.reason}," +
                $" Purchase failure details: {failureDescription.message}");
            callbackFailed?.Invoke();
        }
        */
}
