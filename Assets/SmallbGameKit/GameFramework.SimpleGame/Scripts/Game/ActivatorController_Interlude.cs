using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/ActivatorController_Interlude")]
	public class ActivatorController_Interlude : MonoBehaviour 
	{
		public UniActivation.Activator activator;

		int SelectedIndex
		{
			get
			{
				return (Game.Instance.State == EGameState.Interlude)?1:0;
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
