using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using GameFramework;

using UniPurchase;

using Juicy;

namespace SmallbGameKit
{
	[AddComponentMenu("SmallbGameKit/JuicySDK/JuicySDKPurchaseCaller")]
	public class JuicySDKPurchaseCaller : PurchaseCaller
	{
		public override bool Initialized
		{
			get
			{
				return true;
			}
		}

		public override bool RestorePurchasesSupported
		{
			get
			{
				return JuicySDK.RestorePurchasesSupported;
			}
		}

		public override void Initialize()
		{
			JuicySDK.AddProductDeliveryListener(OnProductDelivery);
			JuicySDK.AddRemoveAdsListener(OnRemoveAds);
		}

		public override void Terminate()
		{
			JuicySDK.RemoveProductDeliveryListener(OnProductDelivery);
			JuicySDK.RemoveRemoveAdsListener(OnRemoveAds);
		}

		public override void BuyProduct(string productId)
		{
			JuicySDK.BuyProduct(productId);
		}

		public override void BuyRemoveAds()
		{
			JuicySDK.BuyRemoveAds();
		}

		public override void RestorePurchases()
		{
			JuicySDK.RestorePurchases();
		}

		void OnProductDelivery(Juicy.ProductSummary productSummary)
		{
			NotifyProductDelivery(ConvertJuicySDKProductToJuicyGameKitProduct(productSummary));
		}

		void OnRemoveAds()
		{
			NotifyRemoveAdsDelivery();
		}

		UniPurchase.ProductSummary ConvertJuicySDKProductToJuicyGameKitProduct(Juicy.ProductSummary sdkProduct)
		{
			UniPurchase.ProductSummary gameKitProduct = new UniPurchase.ProductSummary();

			gameKitProduct.productId = sdkProduct.productId;
			gameKitProduct.productType = (UniPurchase.ProductType)((int)sdkProduct.productType);
			gameKitProduct.localizedPriceString = sdkProduct.localizedPriceString;
			gameKitProduct.localizedPrice = sdkProduct.localizedPrice;
			gameKitProduct.receipt = sdkProduct.receipt;
			gameKitProduct.isoCurrencyCode = sdkProduct.isoCurrencyCode;

			return gameKitProduct;
		}
	}
}
