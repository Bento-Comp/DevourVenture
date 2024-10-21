using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniPurchase
{
	[AddComponentMenu("UniPurchase/PurchaseCaller")]
	public abstract class PurchaseCaller : MonoBehaviour
	{			
		public System.Action<ProductSummary> onProductDelivery;

		public abstract bool Initialized
		{
			get;
		}

		public abstract bool RestorePurchasesSupported
		{
			get;
		}

		public abstract void Initialize();

		public abstract void Terminate();

		public abstract void BuyProduct(string productId);

		public abstract void BuyRemoveAds();

		public abstract void RestorePurchases();

		protected void NotifyProductDelivery(ProductSummary productSummary)
		{
			Debug.Log("PurchaseCaller : NotifyProductDelivery : productID = " + productSummary.productId);

			PurchaseManager.Instance.NotifyProductDelivery(productSummary);
		}

		protected void NotifyRemoveAdsDelivery()
		{
			Debug.Log("PurchaseCaller : NotifyRemoveAdsDelivery");

			PurchaseManager.Instance.NotifyRemoveAdsDelivery();
		}

		void Awake()
		{
			PurchaseManager.Instance.SetCaller(this);
		}
	}
}
