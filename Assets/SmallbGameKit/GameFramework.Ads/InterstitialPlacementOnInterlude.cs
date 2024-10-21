using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.Ads
{
	[AddComponentMenu("GameFramework/Ads/InterstitialPlacementOnInterlude")]
	public class InterstitialPlacementOnInterlude : GameBehaviour
	{
		protected override void OnInterlude()
		{
			base.OnInterlude();

			if(UniAds.AdsManager.Instance == null)
				return;

			UniAds.AdsManager.Instance.NotifyInterstitialPlacementOpportunity();
		}
	}
}