using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UniGameMode;

namespace GameFramework.SimpleGame.GameMode
{
	[DefaultExecutionOrder(-31999)]
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/LevelManager_GameMode")]
	public class LevelManager_GameMode : LevelManager
	{
		public string defaultLevelGameMode = "default";

		// use this if soft game restart
		public bool notifyLevelIndexChangeOnGameModeChange;

		protected override string LevelIndexSaveKey
		{
			get
			{
				string gameMode = GameModeManager.Instance.GameMode;
				if(gameMode == defaultLevelGameMode)
				{
					return base.LevelIndexSaveKey;
				}
				else
				{
					return base.LevelIndexSaveKey + "_" + GameModeManager.Instance.GameMode;
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
			if(notifyLevelIndexChangeOnGameModeChange)
				NotifyLevelIndexChange();
		}
	}
}