using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(-31999)]
	[AddComponentMenu("GameFramework/SimpleGame/MoneyRewardManager")]
	public class MoneyRewardManager : GameBehaviour
	{
		public static System.Action onRewardBonusAdditionGameplayChange;

		public bool rewardActive = true;

		public bool freeReward = false;

		[SerializeField]
		string money = "Gem";

		[SerializeField]
		int rewardAvailableMultiply = 3;

		[SerializeField]
		int baseReward_onSuccess = 40;

		[SerializeField]
		int baseReward_onFail = 40;

		public float canRefuseRewardDelay = 2.0f;

		public bool giveRewardOnFail = true;

		[Header("Debug")]
		public bool debug_forceRewardValue;
		public int debug_forcedRewardValue = 40;

		int currentReward;

		bool hasReward;

		int rewardBonusAdditionGameplay = 0;

		static MoneyRewardManager instance;
		
		public static MoneyRewardManager Instance
		{
			get
			{
				return instance;
			}

		}

		public static bool HasReward => (instance == null)? false :
			(instance.hasReward && instance.rewardActive);

		public static bool HasFreeReward => (instance == null)? false :
			(instance.hasReward && instance.rewardActive && instance.freeReward);

		public float RewardBonusMultiplicateurGameplay {get; set;} = 1.0f;

		public float RewardBonusMultiplicateurLevelEnd {get; set;} = 1.0f;

		public float RewardBonusMultiplicateurAds {get; set;} = 1.0f;

		public int RewardBonusAdditionGameplay
		{
			get => rewardBonusAdditionGameplay;
			set
			{
				if(rewardBonusAdditionGameplay == value)
					return;

				rewardBonusAdditionGameplay = value;
				onRewardBonusAdditionGameplayChange?.Invoke();
			}
		}

		public int CurrentReward
		{
			get
			{
				#if UNITY_EDITOR
				if(debug_forceRewardValue)
				{
					return debug_forcedRewardValue;
				}
				#endif
				return
					Mathf.FloorToInt(
					Mathf.FloorToInt(currentReward
					* RewardBonusMultiplicateurGameplay
					* RewardBonusMultiplicateurLevelEnd
					+ RewardBonusAdditionGameplay) * RewardBonusMultiplicateurAds);
			}
		}

		public string CurrentMoney
		{
			get
			{
				return money;
			}
		}

		public int CurrentAvailableMultiply
		{
			get
			{
				return rewardAvailableMultiply;
			}
		}

		public void MultiplyCurrentReward()
		{
			RewardBonusMultiplicateurAds = CurrentAvailableMultiply;
		}

		public void GiveReward()
		{
			UniMoney.MoneyManager.Instance.AddMoney(CurrentMoney, CurrentReward);
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
				#if UNITY_EDITOR
				if(Application.isPlaying == false)
					return;
				#endif

				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}
		
		protected override void OnAwakeEnd()
		{
			base.OnAwakeEnd();

			if(instance == this)
			{
				instance = null;
			}
		}

		protected override void OnLevelCompleted(bool success)
		{
			base.OnLevelCompleted(success);

			if(giveRewardOnFail == false && success == false)
			{
				hasReward = false;
				currentReward = 0;
				return;
			}

			hasReward = true;
			currentReward = success ? baseReward_onSuccess : baseReward_onFail;
		}
	}
}
