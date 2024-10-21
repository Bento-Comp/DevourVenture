using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework.SimpleGame.MoneyUIGiverInternal;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/MoneyUIGiver")]
	public class MoneyUIGiver : MonoBehaviour
	{
		public System.Action<int, string> onGiveMoney;

		public System.Action onGiveMoneyStart;

		public System.Action onGiveMoneyEnd;

		System.Action onGiveMoneyEnd_user;

		public void GiveMoney(int moneyValue, string moneyName, System.Action onGiveMoneyEnd)
		{
			this.onGiveMoneyEnd_user = onGiveMoneyEnd;
			onGiveMoney?.Invoke(moneyValue, moneyName);
			onGiveMoneyStart?.Invoke();
		}

		public void NotifyGiveMoneyEnd()
		{
			onGiveMoneyEnd_user?.Invoke();
			onGiveMoneyEnd_user = null;

			onGiveMoneyEnd?.Invoke();
		}
	}
}