using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UniPurchase;

namespace UniAds
{
	[AddComponentMenu("UniAds/RemoveAdsOnRemoveAdsDelivery")]
	public class RemoveAdsOnRemoveAdsDelivery : MonoBehaviour
	{
		void Awake()
		{
			PurchaseManager.onRemoveAdsDelivery += OnRemoveAdsDelivery;
		}

		void OnDestroy()
		{
			PurchaseManager.onRemoveAdsDelivery -= OnRemoveAdsDelivery;
		}

		void OnRemoveAdsDelivery()
		{
			RemoveAdsManager.Instance.RemoveAds();
		}
	}
}