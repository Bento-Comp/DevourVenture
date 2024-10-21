using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniActivation;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(-31000)]
	[AddComponentMenu("GameFramework/SimpleGame/DeactivateIfNoMoreSkinToRewardActivator")]
	public class DeactivateIfNoMoreSkinToRewardActivator : MonoBehaviour
	{
		public Activator activator;

		void Awake()
		{
			activator.SelectedIndex = 1;
			if(SkinRewardManager.Instance != null && SkinRewardManager.Instance.SkinToRewardHasBeenSelected)
			{
				OnSelectSkinToReward();
			}
			else
			{
				SkinRewardManager.onSelectSkinToReward += OnSelectSkinToReward;
			}
		}

		void OnDestroy()
		{
			SkinRewardManager.onSkinUnlocked -= OnSelectSkinToReward;
		}

		void OnSelectSkinToReward()
		{
			activator.SelectedIndex = SkinRewardManager.Instance.SkinToRewardAvailable?0:1;
		}
	}
}