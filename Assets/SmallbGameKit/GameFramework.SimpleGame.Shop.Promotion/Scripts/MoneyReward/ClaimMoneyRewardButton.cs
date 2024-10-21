using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework;
using UniUI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/ClaimMoneyRewardButton")]
	public class ClaimMoneyRewardButton : MenuButton
	{
		public override void OnClick()
		{
			base.OnClick();

			MoneyRewardManager.Instance.GiveReward();
			Game.Instance.AskForRestart();
		}
	}
}
