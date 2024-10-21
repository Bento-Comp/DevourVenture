using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

using Juicy;

namespace JuicyInternal
{
	[DefaultExecutionOrder(-31699)]
	[AddComponentMenu("JuicySDKInternal/JuicyPurchaseRemoveAdsManager")]
	public class JuicyPurchaseRemoveAdsManager : MonoBehaviour
	{
		static JuicyPurchaseRemoveAdsManager instance;

		public static JuicyPurchaseRemoveAdsManager Instance
		{
			get
			{
				return instance;
			}
		}

		public void BuyRemoveAds()
		{
			JuicySDKLog.Verbose("JuicyPurchaseManager : BuyRemoveAds");
			JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("purchase_removeAds");
			JuicyPurchaseManager.Instance.BuyProduct(RemoveAdsProductId);
		}

		string RemoveAdsProductId
		{
			get
			{
				return JuicySDK.Settings.RemoveAdsProductID;
			}
		}
			
		void Awake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				JuicySDKLog.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}

			ProductInfos productInfos = new ProductInfos(RemoveAdsProductId,Juicy.ProductType.Consumable);
			JuicyPurchaseManager.onProductDelivery += OnProductDelivery;
		}

		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}

			JuicyPurchaseManager.onProductDelivery -= OnProductDelivery;
		}

		void OnProductDelivery(ProductSummary productSummary)
		{
			if(productSummary.productId == RemoveAdsProductId)
			{
				JuicySDKLog.Verbose("JuicyPurchaseRemoveAdsManager : RemoveAds : " + RemoveAdsProductId);
				JuicyRemoveAdsManager.Instance.RemoveAds();
			}
		}
	}
}
