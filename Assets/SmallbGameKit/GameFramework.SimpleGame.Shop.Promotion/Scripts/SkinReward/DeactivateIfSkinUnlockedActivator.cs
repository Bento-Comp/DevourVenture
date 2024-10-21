using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniActivation;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/DeactivateIfSkinUnlockedActivator")]
	public class DeactivateIfSkinUnlockedActivator : MonoBehaviour
	{
		public Activator activator;

		public void OnCloseSkinRewardScreen()
		{
			activator.SelectedIndex = 0;
		}

		void Awake()
		{
			activator.SelectedIndex = 0;
			if(SkinRewardManager.Instance != null && SkinRewardManager.Instance.SkinHasBeenUnlocked)
			{
				OnSkinUnlocked();
			}
			else
			{
				SkinRewardManager.onSkinUnlocked += OnSkinUnlocked;
			}
		}

		void OnDestroy()
		{
			SkinRewardManager.onSkinUnlocked -= OnSkinUnlocked;
		}

		void OnSkinUnlocked()
		{
			activator.SelectedIndex = 1;
		}
	}
}