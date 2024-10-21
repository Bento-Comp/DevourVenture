using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/FreeSkinRewardActivator")]
	public class FreeSkinRewardActivator : MonoBehaviour
	{
		public UniActivation.Activator activator;

		void Awake()
		{
			activator.SelectedIndex = SkinRewardManager.HasFreeReward?1:0;
		}
	}
}