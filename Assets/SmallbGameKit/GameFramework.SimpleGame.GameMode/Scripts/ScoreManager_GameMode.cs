using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UniGameMode;

namespace GameFramework.SimpleGame.GameMode
{
	[DefaultExecutionOrder(-31999)]
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/ScoreManager")]
	public class ScoreManager_GameMode : ScoreManager
	{
		public string defaultScoreGameMode = "default";

		// use this if soft game restart
		public bool notifyBestScoreChangeOnGameModeChange;

		protected override string BestScoreSaveKey
		{
			get
			{
				string gameMode = GameModeManager.Instance.GameMode;
				if(gameMode == defaultScoreGameMode)
				{
					return base.BestScoreSaveKey;
				}
				else
				{
					return base.BestScoreSaveKey + "_" + GameModeManager.Instance.GameMode;
				}
			}
		}
			
		protected override void OnAwake()
		{
			base.OnAwake();

			GameModeManager.onGameModeChange += OnGameModeChange;
		}
		
		protected override void OnAwakeEnd()
		{
			base.OnAwakeEnd();

			GameModeManager.onGameModeChange -= OnGameModeChange;
		}

		void OnGameModeChange()
		{
			if(notifyBestScoreChangeOnGameModeChange)
				NotifyBestScoreChange();
		}
	}
}