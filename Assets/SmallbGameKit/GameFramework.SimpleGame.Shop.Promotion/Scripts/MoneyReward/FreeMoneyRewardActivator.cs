using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/FreeMoneyRewardActivator")]
	public class FreeMoneyRewardActivator : MonoBehaviour
	{
		public UniActivation.Activator activator;

		void Awake()
		{
			activator.SelectedIndex = MoneyRewardManager.HasFreeReward?1:0;
		}
	}
}