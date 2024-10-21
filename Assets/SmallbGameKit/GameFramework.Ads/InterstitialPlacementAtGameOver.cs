using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.Ads
{
	[AddComponentMenu("GameFramework/Ads/InterstitialPlacementAtGameOver")]
	public class InterstitialPlacementAtGameOver : GameBehaviour
	{
		protected override void OnGameOver()
		{
			if(UniAds.AdsManager.Instance == null)
				return;

			UniAds.AdsManager.Instance.NotifyInterstitialPlacementOpportunity();
		}
	}
}