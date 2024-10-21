using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/ActivatorController_GameState")]
	public class ActivatorController_GameState : MonoBehaviour 
	{
		public UniActivation.Activator activator;

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
			activator.SetFirstActiveState((int)Game.Instance.State);
		}

		void OnGameStateChange()
		{
			activator.SelectedIndex = (int)Game.Instance.State;
		}
	}
}
