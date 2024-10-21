using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.Ads
{
	[AddComponentMenu("GameFramework/Ads/InterstitialPlacementOnLoadGameEnd")]
	public class InterstitialPlacementOnLoadGameEnd : GameBehaviour
	{
		protected override void OnLoadGameEnd(bool reloadSceneAfter)
		{
			if(UniAds.AdsManager.Instance == null)
				return;

			UniAds.AdsManager.Instance.NotifyInterstitialPlacementOpportunity();
		}
	}
}