using UnityEngine;

using UniUI.UniActivation;

using System.Collections.Generic;

using UniGameMode;

namespace GameFramework.SimpleGame.GameMode
{
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/GameModeRadioButtons_ActivationByGameModeUnlockedState")]
	public class GameModeRadioButtons_ActivationByGameModeUnlockedState : MonoBehaviour
	{
		public GameModeRadioButtons radioButtons;

		void Start()
		{
			List<bool> activations = new List<bool>();

			foreach(GameModeRadioButton button in radioButtons.buttons)
			{
				activations.Add(GameModeUnlocker.Instance.IsGameModeUnlocked(button.gameMode));
			}

			radioButtons.activations.SetActivations(activations);
		}
	}
}