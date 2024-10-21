using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniPurchase
{
	public enum ForceRestorePurchaseSupportMode
	{
		DontForce,
		ForceSupported,
		ForceNotSupported
	}

	public enum ForceBuyProductMode
	{
		DontForce,
		ForceSuccess,
		ForceFailure
	}

	[AddComponentMenu("UniPurchase/PurchaseManager")]
	public class PurchaseManager : MonoBehaviour
	{
		public static Action<ProductSummary> onProductDelivery;

		public static Action onRemoveAdsDelivery;

		public ForceRestorePurchaseSupportMode editor_forceRestorePurchaseSupportMode = ForceRestorePurchaseSupportMode.ForceSupported;

		public ForceBuyProductMode editor_forceBuyProductMode = ForceBuyProductMode.ForceSuccess;

		PurchaseCaller caller;

		static PurchaseManager instance;

		public static PurchaseManager Instance
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
				if(caller == null)
					return false;

				return caller.Initialized;
			}
		}

		public bool RestorePurchasesSupported
		{
			get
			{
				if(caller == null)
					return false;

				#if UNITY_EDITOR
				switch(editor_forceRestorePurchaseSupportMode)
				{
					case ForceRestorePurchaseSupportMode.ForceSupported:
						return true;

					case ForceRestorePurchaseSupportMode.ForceNotSupported:
						return false;
				}
				#endif

				return caller.RestorePurchasesSupported;
			}
		}

		public void SetCaller(PurchaseCaller caller)
		{
			if(this.caller != null)
				this.caller.Terminate();

			this.caller = caller;

			if(this.caller != null)
				this.caller.Initialize();
		}

		public void NotifyProductDelivery(ProductSummary productSummary)
		{
			Debug.Log("PurchaseManager : NotifyProductDelivery : productID = " + productSummary.productId);

			onProductDelivery?.Invoke(productSummary);
		}

		public void NotifyRemoveAdsDelivery()
		{
			Debug.Log("PurchaseManager : NotifyRemoveAdsDelivery");

			onRemoveAdsDelivery?.Invoke();
		}

		public void BuyProduct(string productId)
		{
			Debug.Log("PurchaseManager : BuyProduct : " + productId);

			#if UNITY_EDITOR
			if(editor_forceBuyProductMode == ForceBuyProductMode.ForceFailure)
				return;

			if(editor_forceBuyProductMode == ForceBuyProductMode.ForceSuccess)
			{
				NotifyProductDelivery(new ProductSummary(productId));
				return;
			}
			#endif

			if(Initialized)
			{
				caller.BuyProduct(productId);
			}
			else
			{
				Debug.Log("PurchaseManager : BuyProductID FAIL. Not initialized.");
			}
		}

		public void BuyRemoveAds()
		{
			Debug.Log("PurchaseManager : BuyRemoveAds");

			#if UNITY_EDITOR
			if(editor_forceBuyProductMode == ForceBuyProductMode.ForceFailure)
				return;

			if(editor_forceBuyProductMode == ForceBuyProductMode.ForceSuccess)
			{
				NotifyRemoveAdsDelivery();
				return;
			}
			#endif

			if(Initialized)
			{
				caller.BuyRemoveAds();
			}
			else
			{
				Debug.Log("PurchaseManager : BuyRemoveAds FAIL. Not initialized.");
			}
		}


		public void RestorePurchases()
		{
			Debug.Log("PurchaseManager : RestorePurchases");

			if(Initialized == false)
			{
				Debug.Log("PurchaseManager : RestorePurchases FAIL. Not initialized.");
				return;
			}

			if(RestorePurchasesSupported)
			{
				// ... begin restoring purchases
				Debug.Log("PurchaseManager : RestorePurchases started ...");

				caller.RestorePurchases();
			}
			else
			{
				Debug.Log("PurchaseManager : RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
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
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}

		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}
	}
}
