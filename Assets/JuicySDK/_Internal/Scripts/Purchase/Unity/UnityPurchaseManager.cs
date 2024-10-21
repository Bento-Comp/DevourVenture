using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

using Juicy;

namespace JuicyInternal
{
    [DefaultExecutionOrder(-31700)]
    [AddComponentMenu("JuicySDKInternal/UnityPurchaseManager")]
    public class UnityPurchaseManager : PurchaseManagerBase, IStoreListener
    {
        static IStoreController storeController;
        static IExtensionProvider storeExtensionProvider;

        List<ProductSummary> productSummaryList = new List<ProductSummary>();

        public override bool Initialized
        {
            get
            {
                return IsInitialized();
            }
        }

        public override bool RestorePurchasesSupported
        {
            get
            {
                return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer;
            }
        }


        public override void Initialize(List<ProductInfos> products)
        {
            if (storeController == null)
            {
                // If we have already connected to Purchasing ...
                if (IsInitialized())
                {
                    // ... we are done here.
                    return;
                }

                // Create a builder, first passing in a suite of Unity provided stores.
                var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

                foreach (ProductInfos product in products)
                {
                    builder.AddProduct(product.productId, GetUnityPurchasingProductTypeFromJuicySDKProductType(product.type));
                }

                // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
                // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
                UnityPurchasing.Initialize(this, builder);
            }
        }

        public override void Terminate()
        {
        }

        public override void BuyProduct(string productId)
        {
            JuicySDKLog.Verbose("UnityPurchaseCaller : BuyProduct : " + productId);

            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Product product = storeController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    JuicySDKLog.Verbose(string.Format("UnityPurchaseCaller : Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.

                    storeController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    JuicySDKLog.Verbose("UnityPurchaseCaller : BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                JuicySDKLog.Verbose("UnityPurchaseCaller : BuyProductID FAIL. Not initialized.");
            }
        }

        // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google.
        // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
        public override void RestorePurchases()
        {
            JuicySDKLog.Verbose("UnityPurchaseCaller : RestorePurchases");

            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                JuicySDKLog.Verbose("UnityPurchaseCaller : RestorePurchases FAIL. Not initialized.");
                return;
            }

            // ... begin restoring purchases
            JuicySDKLog.Verbose("UnityPurchaseCaller : RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                JuicySDKLog.Verbose("PurchaseManager : RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }

        //  
        // --- IStoreListener
        //

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            JuicySDKLog.Verbose("PurchaseManager : OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            storeController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            storeExtensionProvider = extensions;

            // hack retreive list of products
            foreach (Product product in storeController.products.all)
            {
                ProductSummary productSummary = CreateJuicySDKProductSummaryFromUnityPurchasingProduct(product);
                productSummaryList.Add(productSummary);
            }
        }


        public override List<ProductSummary> GetProductSummaryList()
        {
            return productSummaryList;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            JuicySDKLog.Verbose("PurchaseManager : OnInitializeFailed InitializationFailureReason:" + error);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            JuicySDKLog.Verbose("PurchaseManager : OnInitializeFailed InitializationFailureReason:" + error);
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            ProcessDelivery(args.purchasedProduct);

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
            // this reason with the user to guide their troubleshooting actions.
            JuicySDKLog.Verbose(string.Format("PurchaseManager : OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

            NotifyProductDelivery(CreateJuicySDKProductSummaryFromUnityPurchasingProduct(product), false);
            JuicyPurchaseManager.onPurchaseFail?.Invoke();
        }

        bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return storeController != null && storeExtensionProvider != null;
        }

        public void ProcessDelivery(Product product)
        {
            JuicySDKLog.Verbose("PurchaseManager : ProcessDelivery : " + product.definition.id);
            NotifyProductDelivery(CreateJuicySDKProductSummaryFromUnityPurchasingProduct(product), true);
        }

        public Juicy.ProductType GetJuicySDKProductTypeFromUnityPurchasingProductType(UnityEngine.Purchasing.ProductType productType)
        {
            switch (productType)
            {
                case UnityEngine.Purchasing.ProductType.Consumable:
                    return Juicy.ProductType.Consumable;

                case UnityEngine.Purchasing.ProductType.NonConsumable:
                    return Juicy.ProductType.NonConsumable;
            }

            return Juicy.ProductType.Consumable;
        }

        public UnityEngine.Purchasing.ProductType GetUnityPurchasingProductTypeFromJuicySDKProductType(Juicy.ProductType productType)
        {
            switch (productType)
            {
                case Juicy.ProductType.Consumable:
                    return UnityEngine.Purchasing.ProductType.Consumable;

                case Juicy.ProductType.NonConsumable:
                    return UnityEngine.Purchasing.ProductType.NonConsumable;
            }

            return UnityEngine.Purchasing.ProductType.Consumable;
        }

        ProductSummary CreateJuicySDKProductSummaryFromUnityPurchasingProduct(UnityEngine.Purchasing.Product product)
        {
            ProductSummary productSummary = new ProductSummary();
            productSummary.productId = product.definition.id;
            productSummary.productType = GetJuicySDKProductTypeFromUnityPurchasingProductType(product.definition.type);
            productSummary.transactionId = product.transactionID;
            productSummary.localizedPriceString = product.metadata.localizedPriceString;
            productSummary.receipt = product.receipt;
            productSummary.localizedPrice = product.metadata.localizedPrice;
            productSummary.isoCurrencyCode = product.metadata.isoCurrencyCode;

            return productSummary;
        }
    }
}
