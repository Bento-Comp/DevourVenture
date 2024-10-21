using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.Ads
{
	[AddComponentMenu("GameFramework/Ads/InterstitialPlacementOnLevelCompleted")]
	public class InterstitialPlacementOnLevelCompleted : GameBehaviour
	{
		protected override void OnLevelCompleted(bool success)
		{
			base.OnLevelCompleted(success);

			if(UniAds.AdsManager.Instance == null)
				return;

			UniAds.AdsManager.Instance.NotifyInterstitialPlacementOpportunity();
		}
	}
}