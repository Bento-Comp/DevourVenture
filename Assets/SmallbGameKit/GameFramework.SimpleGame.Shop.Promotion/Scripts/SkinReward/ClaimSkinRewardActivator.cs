using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/ClaimSkinRewardActivator")]
	public class ClaimSkinRewardActivator : MonoBehaviour
	{
		public UniActivation.Activator activator;

		public void NotifyRewardObtained()
		{
			activator.SelectedIndex = 1;
		}

		void Awake()
		{
			activator.SelectedIndex = 0;
		}
	}
}