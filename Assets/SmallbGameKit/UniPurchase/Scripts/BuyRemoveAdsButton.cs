using UnityEngine;
using System.Collections;

using UniUI;

using UniAds;

namespace UniPurchase
{
	[AddComponentMenu("UniPurchase/BuyRemoveAdsButton")]
	public class BuyRemoveAdsButton : MenuButton
	{
		public GameObject root;
		
		public override void OnClick()
		{
			PurchaseManager.Instance.BuyRemoveAds();
		}

		protected override void OnAwake()
		{
			RemoveAdsManager.onRemoveAds += OnRemoveAds;
			OnRemoveAds();
		}


		protected override void OnAwakeEnd()
		{
			RemoveAdsManager.onRemoveAds -= OnRemoveAds;
		}

		void OnRemoveAds()
		{
			UpdateActivation();
		}

		void UpdateActivation()
		{
			if(RemoveAdsManager.Instance.AdsRemoved)
				Destroy(root);
		}
	}
}