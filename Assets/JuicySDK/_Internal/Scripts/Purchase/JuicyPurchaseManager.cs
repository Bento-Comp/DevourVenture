using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

using Juicy;

namespace JuicyInternal
{
    [DefaultExecutionOrder(-31700)]
    [AddComponentMenu("JuicySDKInternal/JuicyPurchaseManager")]
    public class JuicyPurchaseManager : MonoBehaviour
    {
        public static Action<ProductSummary> onProductDelivery;
        public static Action onPurchaseFail;

        PurchaseManagerBase purchaseManager;

        bool purchaseInProgress;

        const float purchaseTimeOut = 60.0f;

        static JuicyPurchaseManager instance;

        public static JuicyPurchaseManager Instance
        {
            get
            {
                return instance;
            }
        }

        public bool Initialized
        {
            get
            {
                if (purchaseManager == null)
                    return false;

#if UNITY_EDITOR
				return true;
#else
                return purchaseManager.Initialized;
#endif
            }
        }

        public bool RestorePurchasesSupported
        {
            get
            {
#if UNITY_EDITOR
				if(JuicySDK.Settings.RestorePurchaseAvailableInEditor)
				{
					return true;
				}
				else
#endif
                {
                    return purchaseManager.RestorePurchasesSupported;
                }
            }
        }

        public List<ProductSummary> GetProductSummaryList()
        {
            return purchaseManager.GetProductSummaryList();
        }

        public void BuyProduct(string productId)
        {
            JuicySDKLog.Verbose("JuicyPurchaseManager : BuyProduct : productId = " + productId + " | purchaseInProgress = " + purchaseInProgress);

            if (purchaseInProgress)
            {
                JuicySDKLog.Verbose("JuicyPurchaseManager : BuyProduct : productId = " + productId + " : Exit because a purchase is already in progress");
                return;
            }

            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("buy_opportunity", new EventProperty("productId", productId));
            if (Initialized)
            {
                PurchaseBegin();

                JuicySDKLog.Verbose("JuicyPurchaseManager : BuyProduct : productId = " + productId);

                JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("buy_trigger", new EventProperty("productId", productId));
#if UNITY_EDITOR
				if(JuicySDK.Settings.ForcePurchaseCancelInEditor)
				{
					OnProductDelivery(new ProductSummary(productId), false);
					return;
				}
#endif
                purchaseManager.BuyProduct(productId);
            }
            else
            {
                OnProductDelivery(new ProductSummary(productId), false); //hack : purchase fail

                JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("buy_opport_noInit", new EventProperty("productId", productId));
                JuicySDKLog.Verbose("JuicyPurchaseManager : BuyProduct : productId = " + productId + " : Fail. Not initialized.");
            }
        }

        public void RestorePurchases()
        {
            JuicySDKLog.Verbose("JuicyPurchaseManager : RestorePurchases");

            if (purchaseInProgress)
            {
                JuicySDKLog.Verbose("JuicyPurchaseManager : RestorePurchases : Exit because a purchase is already in progress");
                return;
            }

            if (Initialized == false)
            {
                JuicySDKLog.Verbose("JuicyPurchaseManager : RestorePurchases : Fail. Not initialized.");
                return;
            }

            if (RestorePurchasesSupported)
            {
                // ... begin restoring purchases
                JuicySDKLog.Verbose("JuicyPurchaseManager : RestorePurchases : started ...");

                PurchaseBegin();

                purchaseManager.RestorePurchases();
            }
            else
            {
                JuicySDKLog.Verbose("JuicyPurchaseManager : RestorePurchases : Fail. Not supported on this platform. Current = " + Application.platform);
            }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                JuicySDKLog.LogWarning("A singleton can only be instantiated once!");
                Destroy(gameObject);
                return;
            }

            CreateUnityPurchase();
        }

        void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        void OnProductDelivery(ProductSummary productSummary, bool success)
        {
            JuicySDKLog.Verbose("JuicyPurchaseManager : OnProductDelivery : productID = " + productSummary.productId + " | success = " + success);

            if (success)
            {
                if (onProductDelivery != null)
                {
                    onProductDelivery(productSummary);
                }
            }
            else
            {
                //hack : purchase fail
                onPurchaseFail?.Invoke();
            }

            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("delivery", new EventProperty("productId", productSummary.productId), new EventProperty("success", success));
            PurchaseEnd();
        }

        void CreateUnityPurchase()
        {
            Select(JuicyUtility.CreateSubManager<UnityPurchaseManager>(transform));
        }

        void Select(PurchaseManagerBase purchaseManagerToSelect)
        {
            if (this.purchaseManager != null)
            {
                this.purchaseManager.onProductDelivery -= OnProductDelivery;
                this.purchaseManager.Terminate();
            }

            this.purchaseManager = purchaseManagerToSelect;

            if (this.purchaseManager != null)
            {
                this.purchaseManager.Initialize(JuicySDK.Settings.Products);
                this.purchaseManager.onProductDelivery += OnProductDelivery;
            }
        }

        void PurchaseTimeOut()
        {
            JuicySDKLog.Verbose("JuicyPurchaseManager : PurchaseTimeOut");

            PurchaseEnd();
        }

        void PurchaseBegin()
        {
            JuicySDKLog.Verbose("JuicyPurchaseManager : PurchaseBegin");
            purchaseInProgress = true;
            CancelInvoke("PurchaseTimeOut");
            Invoke("PurchaseTimeOut", purchaseTimeOut);
        }

        void PurchaseEnd()
        {
            if (purchaseInProgress == false)
                return;

            JuicySDKLog.Verbose("JuicyPurchaseManager : PurchaseEnd");

            purchaseInProgress = false;
        }
    }
}
