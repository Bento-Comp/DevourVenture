using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UniGameMode;

namespace GameFramework.SimpleGame.GameMode
{
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/SelectModeRadioButton")]
	public class SelectModeRadioButton : ToggleButton
	{
		public string gameMode;

		public bool restartOnSelectMode;

		public override void OnValueChange(bool value)
		{
			if(value)
			{
				bool gameModeChanged = GameModeManager.Instance.EnableGameMode(gameMode);

				if(gameModeChanged == false)
					return;

				if(restartOnSelectMode)
					Game.Instance.DoRestart();
			}
		}

		protected override void OnAwake()
		{
			Button.isOn = GameModeManager.Instance.IsGameModeEnabled(gameMode);
		}
	}
}
