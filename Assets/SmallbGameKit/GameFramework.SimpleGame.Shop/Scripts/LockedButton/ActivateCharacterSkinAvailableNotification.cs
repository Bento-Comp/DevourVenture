using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

using UniActivation;
using UniMoney;
using UniSkin;
using GameFramework.SimpleGame.Skin;

namespace GameFramework.SimpleGame
{
    [DefaultExecutionOrder(1)]
	[AddComponentMenu("GameFramework/SimpleGame/ActivateCharacterSkinAvailableNotification")]
	public class ActivateCharacterSkinAvailableNotification : MonoBehaviour
	{
		public Activator activator;

		public SkinSelectorsSpawner skinSelectorsSpawner;

		bool firstUpdateAfterEnable;

		bool IsSkinAvailable
		{
			get
			{
				foreach(SkinSelector skinSelector in skinSelectorsSpawner.SkinSelectors)
				{
					LockedBlock lockedBlock = skinSelector.GetComponent<LockedBlock_Reference>().lockedBlock;

					if(lockedBlock.lockActivator.Unlocked || lockedBlock.SkinCost <= 0)
						continue;

					int money = MoneyManager.Instance.GetMoney(lockedBlock.UnlockMoneyName);

					if(money >= skinSelector.GetComponent<LockedBlock_Reference>().lockedBlock.RemainingCost)
						return true;
				}

				return false;
			}
		}

		void OnEnable()
		{
			firstUpdateAfterEnable = true;
		}

		void LateUpdate()
		{
			if(firstUpdateAfterEnable)
			{
				firstUpdateAfterEnable = false;
				activator.SelectedIndex = IsSkinAvailable ? 1 : 0;
			}
		}
	}
}