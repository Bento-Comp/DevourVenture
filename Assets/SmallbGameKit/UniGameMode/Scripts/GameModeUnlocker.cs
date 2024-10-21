using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniGameMode
{
	[DefaultExecutionOrder(-31999)]
	[AddComponentMenu("UniGameMode/GameModeUnlocker")]
	public class GameModeUnlocker : UniSingleton.Singleton<GameModeUnlocker>
	{
		[System.Serializable]
		public class GameModeUnlockSettings
		{
			public string gameMode = "default";
			public bool unlockedByDefault;

			public GameModeUnlockSettings(string gameMode, bool unlockedByDefault)
			{
				this.gameMode = gameMode;
				this.unlockedByDefault = unlockedByDefault;
			}
		}

		public GameModeManager gameModeManager;

		public List<GameModeUnlockSettings> gameModeUnlockSettings =
			new List<GameModeUnlockSettings>(){new GameModeUnlockSettings("default", true)};

		List<string> unlockedGameModes = new List<string>();

		string gameModeUnlocked_prefix_key = "UniGameMode_GameModeUnlocked_";
		string GetGameModeUnlockedPrefixKey(string gameMode)
			=> gameModeUnlocked_prefix_key + gameMode;

		public List<string> UnlockedGameModes => unlockedGameModes;

		public void UnlockGameMode(string gameMode)
			=> PlayerPrefs.SetInt(GetGameModeUnlockedPrefixKey(gameMode), 1);

		public bool IsGameModeUnlocked(string gameMode)
			=> PlayerPrefs.GetInt(GetGameModeUnlockedPrefixKey(gameMode), 0) == 1;

		void Awake()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			UnlockGameModeByDefault();
		}

		void UnlockGameModeByDefault()
		{
			foreach(GameModeUnlockSettings unlockSettings in gameModeUnlockSettings)
			{
				if(unlockSettings.unlockedByDefault)
					UnlockGameMode(unlockSettings.gameMode);

				if(IsGameModeUnlocked(unlockSettings.gameMode))
					unlockedGameModes.Add(unlockSettings.gameMode);
			}
		}
	}
}