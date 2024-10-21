using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework;
using UniUI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/ClaimSkinRewardButton")]
	public class ClaimSkinRewardButton : MenuButton
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
