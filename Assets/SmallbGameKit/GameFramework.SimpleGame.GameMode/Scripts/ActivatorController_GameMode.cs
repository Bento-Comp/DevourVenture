using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UniGameMode;

using UniActivation;

namespace GameFramework.SimpleGame.GameMode
{
	[DefaultExecutionOrder(-30000)]
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/ActivatorController_GameMode")]
	public class ActivatorController_GameMode : MonoBehaviour 
	{
		public Activator activator;

		public bool onlySelectIndexAtStart;

		public List<string> gameModes;

		int ActivationIndex
		{
			get
			{
				int gameModeIndex = gameModes.IndexOf(GameModeManager.Instance.GameMode);
				if(gameModeIndex < 0)
					gameModeIndex = 0;

				return gameModeIndex;
			}
		}

		void Awake()
		{
			GameModeManager.onGameModeChange += OnGameModeChange;

			InitializeGameState();
		}

		void OnDestroy()
		{
			GameModeManager.onGameModeChange -= OnGameModeChange;
		}

		void InitializeGameState()
		{
			activator.SetFirstActiveState(ActivationIndex);
		}

		void OnGameModeChange()
		{
			if(onlySelectIndexAtStart)
				return;

			activator.SelectedIndex = ActivationIndex;
		}
	}
}
