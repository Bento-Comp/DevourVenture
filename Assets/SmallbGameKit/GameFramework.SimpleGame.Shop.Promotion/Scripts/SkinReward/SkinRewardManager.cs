using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework;

using UniSkin;
using GameFramework.SimpleGame.Skin;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(-31999)]
	[AddComponentMenu("GameFramework/SimpleGame/SkinRewardManager")]
	public class SkinRewardManager : GameBehaviour
	{
		public static System.Action onSkinUnlockProgress;

		public static System.Action onSkinUnlocked;

		public static System.Action onSelectSkinToReward;

		public bool rewardActive = true;

		public bool freeReward = false;

		public int levelToCompleteBeforeUnlockingSkin = 3;

		public float canRefuseRewardDelay = 2.0f;

		public float animateSkinUnlockProgressDuration = 0.5f;

		public float unlockDelay = 1.0f;

		public bool giveRewardOnFail = true;

		public bool giveMoneyRewardEvenWhenThereIsASkinPopUp = true;

		public int skinLayerIndex = 3;

		public SkinSelectorsSpawner skinSelectorsSpawner;

		static SkinRewardManager instance;

		bool skinUnlockHasProgressed;

		bool skinUnlockHasBeenUnlocked;

		float currentSkinUnlockProgressPercent;

		float lastSkinUnlockProgressPercent;

		SkinSelector selectedSkinToRewardSkinSelector;

		bool skinToRewardHasBeenSelected;

		bool hasReward;

		public static bool HasReward => (instance == null)? false
			: (instance.hasReward && instance.rewardActive);

		public static bool HasFreeReward => (instance == null)? false :
			(instance.hasReward && instance.rewardActive && instance.freeReward);

		public static SkinRewardManager Instance
		{
			get
			{
				return instance;
			}
		}

		public bool SkinToRewardHasBeenSelected
		{
			get
			{
				return skinToRewardHasBeenSelected;
			}
		}

		public bool SkinToRewardAvailable
		{
			get
			{
				return selectedSkinToRewardSkinSelector != null;
			}
		}

		public SkinSelector SelectedSkinToRewardSkinSelector
		{
			get
			{
				return selectedSkinToRewardSkinSelector;
			}
		}

		public float SkinUnlockProgressPercent
		{
			get
			{
				return currentSkinUnlockProgressPercent;
			}
		}

		public float LastSkinUnlockProgressPercent
		{
			get
			{
				return lastSkinUnlockProgressPercent;
			}
		}

		public bool SkinUnlockHasProgressed
		{
			get
			{
				return skinUnlockHasProgressed;
			}
		}

		public bool SkinHasBeenUnlocked
		{
			get
			{
				return skinUnlockHasBeenUnlocked;
			}
		}

		bool SkinUnlocked
		{
			get
			{
				return currentSkinUnlockProgress >= levelToCompleteBeforeUnlockingSkin;
			}
		}

		int currentSkinUnlockProgress;
		int CurrentSkinUnlockProgress
		{
			get
			{
				return currentSkinUnlockProgress;
			}

			set
			{
				currentSkinUnlockProgress = value;
				CurrentSkinUnlockProgress_Save = value; 
			}
		}

		static string currentSkinUnlockProgressKey = "SkinRewardManager_CurrentSkinUnlockProgress";
		int CurrentSkinUnlockProgress_Save
		{
			get
			{
				return PlayerPrefs.GetInt(currentSkinUnlockProgressKey, 0);
			}

			set
			{
				PlayerPrefs.SetInt(currentSkinUnlockProgressKey, value);
			}
		}

		static string currentSkinToRewardIndexKey = "SkinRewardManager_CurrentSkinToRewardIndex";
		int CurrentSkinToRewardIndex
		{
			get
			{
				return PlayerPrefs.GetInt(currentSkinToRewardIndexKey, 0);
			}

			set
			{
				PlayerPrefs.SetInt(currentSkinToRewardIndexKey, value);
			}
		}

		public void CheckIfAlsoGiveMoneyReward()
		{
			// If we got the opportunity to get a skin each level
			// (or if we desire to give a reward even when there is a skin popup)
			// give also the base reward when the player dont take the skin
			if(giveMoneyRewardEvenWhenThereIsASkinPopUp
				|| levelToCompleteBeforeUnlockingSkin <= 1)
			{
				MoneyRewardManager.Instance.GiveReward();
			}
		}

		public void LoseReward()
		{
			++CurrentSkinToRewardIndex;
		}

		public void GiveReward()
		{
			selectedSkinToRewardSkinSelector.SelectSkin();

			LockedBlock_Reference lockedBlockReference = selectedSkinToRewardSkinSelector.GetComponent<LockedBlock_Reference>();
			if(lockedBlockReference != null)
			{
				lockedBlockReference.lockedBlock.lockActivator.Unlocked = true;
			}

			++CurrentSkinToRewardIndex;
		}

		protected override void OnAwake()
		{
			base.OnAwake();

			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}

			currentSkinUnlockProgress = CurrentSkinUnlockProgress_Save;

			if(currentSkinUnlockProgress >= levelToCompleteBeforeUnlockingSkin)
				CurrentSkinUnlockProgress = 0;

			currentSkinUnlockProgressPercent = ComputeCurrentSkinUnlockProgressPercent();
			lastSkinUnlockProgressPercent = currentSkinUnlockProgressPercent;

			SelectSkinToReward();

			SkinManager.onSkinChange += OnSkinChange;
		}
		
		protected override void OnAwakeEnd()
		{
			base.OnAwakeEnd();

			if(instance == this)
			{
				instance = null;
				SkinManager.onSkinChange -= OnSkinChange;
			}
		}

		protected override void OnLevelCompleted(bool success)
		{
			base.OnLevelCompleted(success);

			if(rewardActive == false)
				return;

			hasReward = false;

			if(giveRewardOnFail == false && success == false)
			{
				return;
			}

			SelectSkinToReward();

			if(SelectedSkinToRewardSkinSelector == null)
				return;

			hasReward = true;

			IncrementSkinUnlockProgress();
		}

		void OnSkinChange(int skinIndex)
		{
			if(skinIndex != skinLayerIndex)
				return;

			SelectSkinToReward();
		}

		void SelectSkinToReward()
		{
			selectedSkinToRewardSkinSelector = null;
			int startingRewardIndex = CurrentSkinToRewardIndex; 
			int rewardIndex = startingRewardIndex;
			bool loopedAround = false;
			int count = skinSelectorsSpawner.SkinSelectors.Count;

			// Clamp starting reward index
			startingRewardIndex = Mathf.Clamp(startingRewardIndex, 0, count - 1);

			while(loopedAround == false || rewardIndex < startingRewardIndex)
			{
				if(rewardIndex >= count)
				{
					loopedAround = true;
					rewardIndex = 0;
					continue;
				}

				SkinSelector skinSelector = skinSelectorsSpawner.SkinSelectors[rewardIndex];

				LockedBlock lockedBlock = skinSelector.GetComponent<LockedBlock_Reference>().lockedBlock;

				bool canReward = true;

				if(lockedBlock.lockActivator.Unlocked || lockedBlock.SkinCost <= 0)
					canReward = false;

				SkinItem_Bool canBeRewarded = skinSelector.GetSkinItem<SkinItem_Bool>("CanBeRewarded");
				if(canBeRewarded == null || canBeRewarded.GetBool() == false)
					canReward = false;

				if(canReward)
				{
					selectedSkinToRewardSkinSelector = skinSelector;
					break;
				}

				++rewardIndex;
			}

			CurrentSkinToRewardIndex = rewardIndex;

			onSelectSkinToReward?.Invoke();

			skinToRewardHasBeenSelected = true;
		}

		float ComputeCurrentSkinUnlockProgressPercent()
		{
			if(currentSkinUnlockProgress >= levelToCompleteBeforeUnlockingSkin)
				return 1.0f;

			if(currentSkinUnlockProgress <= 0)
				return 0.0f;

			return (float)currentSkinUnlockProgress/(float)levelToCompleteBeforeUnlockingSkin;
		}

		void IncrementSkinUnlockProgress()
		{
			++CurrentSkinUnlockProgress;

			currentSkinUnlockProgressPercent = ComputeCurrentSkinUnlockProgressPercent();

			skinUnlockHasProgressed = true;

			onSkinUnlockProgress?.Invoke();

			if(SkinUnlocked)
			{
				onSkinUnlocked?.Invoke();
				skinUnlockHasBeenUnlocked = true;
			}
		}
	}
}
