using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/ActivatorController_GamePlay")]
	public class ActivatorController_GamePlay : MonoBehaviour 
	{
		public UniActivation.Activator activator;

		int SelectedIndex
		{
			get
			{
				return (Game.Instance.State == EGameState.Play)?1:0;
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
