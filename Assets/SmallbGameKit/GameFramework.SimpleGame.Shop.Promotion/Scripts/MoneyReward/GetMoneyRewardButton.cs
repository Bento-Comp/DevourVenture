using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework;
using UniUI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/GetMoneyRewardButton")]
	public class GetMoneyRewardButton : MenuButton
	{
		public MoneyRewardUI moneyRewardUI;

		public override void OnClick()
		{
			base.OnClick();

			MoneyRewardManager manager = MoneyRewardManager.Instance;

			moneyRewardUI.moneyUIGiver.GiveMoney(manager.CurrentReward, manager.CurrentMoney, OnGiveMoneyEnd);
			moneyRewardUI.deactivator.Deactivate();
		}

		void OnGiveMoneyEnd()
		{
			//moneyRewardUI.deactivator.Deactivate();
		}
	}
}
