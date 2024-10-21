using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/MoneyUIGiver_GiveStartActivator")]
	public class MoneyUIGiver_GiveStartActivator : MonoBehaviour
	{
		public MoneyUIGiver giver;

		public UniActivation.Activator activator;

		void OnEnable()
		{
			activator.SelectedIndex = 0;
			giver.onGiveMoneyStart += OnGiveMoneyStart;
		}

		void OnDisable()
		{
			giver.onGiveMoneyStart -= OnGiveMoneyStart;
		}

		void OnGiveMoneyStart()
		{
			activator.SelectedIndex = 1;
		}
	}
}