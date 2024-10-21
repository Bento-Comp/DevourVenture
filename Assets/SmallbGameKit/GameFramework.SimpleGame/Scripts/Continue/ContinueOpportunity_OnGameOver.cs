using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/ContinueOpportunity_OnGameOver")]
	public class ContinueOpportunity_OnGameOver : GameBehaviour 
	{
		protected override void OnGameOver()
		{
			ContinueManager.Instance.NotifyContinueOpportunity();
		}
	}
}
