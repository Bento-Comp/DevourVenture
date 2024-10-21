using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/MoneyUIGiver_GiveEndActivator")]
	public class MoneyUIGiver_GiveEndActivator : MonoBehaviour
	{
		public MoneyUIGiver giver;

		public UniActivation.Activator activator;

		void OnEnable()
		{
			activator.SelectedIndex = 0;
			giver.onGiveMoneyEnd += OnGiveMoneyEnd;
		}

		void OnDisable()
		{
			giver.onGiveMoneyEnd -= OnGiveMoneyEnd;
		}

		void OnGiveMoneyEnd()
		{
			activator.SelectedIndex = 1;
		}
	}
}