using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/ActivatorController_LevelCompletedWithReward")]
	public class ActivatorController_LevelCompletedWithReward : MonoBehaviour 
	{
		public UniActivation.Activator activator;

		int SelectedIndex
		{
			get
			{
				bool completed = Game.Instance.State == EGameState.LevelCompleted;
				bool hasReward = (MoneyRewardManager.HasReward || SkinRewardManager.HasReward);
				return (completed && hasReward)?1:0;
			}
		}

		void Awake()
		{
			Game.onGameStateChange += OnGameStateChange;

			InitializeGameState();
		}

		void OnDestroy()
		{
			Game.onGameStateChange -= OnGameStateChange;
		}

		void InitializeGameState()
		{
			activator.SetFirstActiveState(SelectedIndex);
		}

		void OnGameStateChange()
		{
			activator.SelectedIndex = SelectedIndex;
		}
	}
}
