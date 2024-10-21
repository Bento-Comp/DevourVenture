using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/GetFreeSkinRewardButton")]
	public class GetFreeSkinRewardButton : MenuButton
	{
		public SkinRewardUnlockedScreenActivator screenActivator;

		public override void OnClick()
		{
			base.OnClick();

			SkinRewardManager.Instance.GiveReward();

			//SkinRewardManager.Instance.CheckIfAlsoGiveMoneyReward();

			//Game.Instance.AskForRestart();
			screenActivator.CloseSkinRewardScreen();
		}
	}
}
