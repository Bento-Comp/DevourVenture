using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework;
using UniUI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/RefuseSkinRewardButton")]
	public class RefuseSkinRewardButton : MenuButton
	{
		public SkinRewardUnlockedScreenActivator screenActivator;

		public override void OnClick()
		{
			base.OnClick();

			SkinRewardManager.Instance.LoseReward();

			//SkinRewardManager.Instance.CheckIfAlsoGiveMoneyReward();

			//Game.Instance.AskForRestart();
			screenActivator.CloseSkinRewardScreen();
		}
	}
}
