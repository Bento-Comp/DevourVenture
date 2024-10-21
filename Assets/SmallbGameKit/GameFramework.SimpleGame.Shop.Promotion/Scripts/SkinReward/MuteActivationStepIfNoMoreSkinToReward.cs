using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniActivation;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/MuteActivationStepIfNoMoreSkinToReward")]
	public class MuteActivationStepIfNoMoreSkinToReward : MonoBehaviour
	{
		public ActivationStep activationStep;

		void Awake()
		{
			activationStep.mute = false;
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
			activationStep.mute = SkinRewardManager.Instance.SkinToRewardAvailable ? false : true;
		}
	}
}