using UnityEngine;

using UniUI.UniActivation;

using UniGameMode;

namespace GameFramework.SimpleGame.GameMode
{
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/GameModeRadioButton")]
	public class GameModeRadioButton : RadioButtonBase
	{
		public string gameMode = "default";

		public bool restartOnSelectMode;

		public override void OnSelect()
		{
			bool gameModeChanged = GameModeManager.Instance.EnableGameMode(gameMode);

			if(gameModeChanged == false)
				return;

			if(restartOnSelectMode)
				Game.Instance.DoRestart();
		}

		protected override void OnAwake()
		{
			if(GameModeManager.Instance.IsGameModeEnabled(gameMode))
				SetInitialRadioButton();
		}
	}
}