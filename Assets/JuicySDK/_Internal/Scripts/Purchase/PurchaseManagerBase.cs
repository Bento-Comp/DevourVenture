using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Juicy;

namespace JuicyInternal
{
	[AddComponentMenu("JuicySDKInternal/PurchaseCaller")]
	public abstract class PurchaseManagerBase : MonoBehaviour
	{			
		public System.Action<ProductSummary, bool> onProductDelivery;

		public abstract bool Initialized
		{
			get;
		}

		public abstract bool RestorePurchasesSupported
		{
			get;
		}

		public abstract void Initialize(List<ProductInfos> products);

		public abstract void Terminate();

		public abstract void BuyProduct(string productId);

		public abstract void RestorePurchases();

		public abstract List<ProductSummary> GetProductSummaryList();

		public void NotifyProductDelivery(ProductSummary productSummary, bool success)
		{
			JuicySDKLog.Verbose("PurchaseCaller : NotifyProductDelivery : productID = " + productSummary.productId + " | success = " + success);

			if(onProductDelivery != null)
				onProductDelivery(productSummary, success);
		}
	}
}
