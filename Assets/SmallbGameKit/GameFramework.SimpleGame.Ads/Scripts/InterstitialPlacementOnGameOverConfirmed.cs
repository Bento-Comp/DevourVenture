using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using GameFramework.SimpleGame;

namespace GameFramework.SimpleGame.Ads
{
	[AddComponentMenu("GameFramework/SimpleGame/Ads/InterstitialPlacementOnGameOverConfirmed")]
	public class InterstitialPlacementOnGameOverConfirmed : MonoBehaviour
	{
		void Awake()
		{
			ContinueManager.onGameOverConfirmed += OnGameOverConfirmed;
		}

		void OnDestroy()
		{
			ContinueManager.onGameOverConfirmed -= OnGameOverConfirmed;
		}
		
		void OnGameOverConfirmed()
		{
			if(UniAds.AdsManager.Instance == null)
				return;

			UniAds.AdsManager.Instance.NotifyInterstitialPlacementOpportunity();
		}
	}
}