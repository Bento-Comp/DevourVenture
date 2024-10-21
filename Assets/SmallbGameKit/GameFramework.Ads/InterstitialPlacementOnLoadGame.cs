using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.Ads
{
	[AddComponentMenu("GameFramework/Ads/InterstitialPlacementOnLoadGame")]
	public class InterstitialPlacementOnLoadGame : GameBehaviour
	{
		public bool notOnFirstLoadGame = true;

		protected override void OnLoadGame()
		{
			if(UniAds.AdsManager.Instance == null)
				return;

			if(notOnFirstLoadGame && Game.IsFirstGameLoadStatic)
				return;

			UniAds.AdsManager.Instance.NotifyInterstitialPlacementOpportunity();
		}
	}
}