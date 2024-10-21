using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniSkin;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/SetSkinToReward")]
	[DefaultExecutionOrder(-1)]
	public class SetSkinToReward : MonoBehaviour
	{
		public SkinUserBase skinUser;

		void Start()
		{
			if(SkinRewardManager.Instance == null)
				return;

			SkinRewardManager.onSelectSkinToReward += OnSelectSkinToReward;
			OnSelectSkinToReward();
		}

		void OnDestroy()
		{
			SkinRewardManager.onSelectSkinToReward -= OnSelectSkinToReward;
		}

		void OnSelectSkinToReward()
		{
			SkinSelector skinSelector = SkinRewardManager.Instance.SelectedSkinToRewardSkinSelector;

			if(skinSelector == null)
				return;

			skinUser.ParentSkinUser = skinSelector;
		}
	}
}