using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniGameMode
{
	[DefaultExecutionOrder(-32000)]
	[ExecuteAlways()]
	[AddComponentMenu("UniGameMode/GameModeManager")]
	public class GameModeManager : MonoBehaviour
	{	
		public static Action onGameModeChange;

		[SerializeField]
		string defaultGameMode = "default";

		public bool forceDefaultGameModeOnFirstAwake;

		static string gameMode_key = "UniGameMode_GameMode";

		public string GameMode
		{
			get => PlayerPrefs.GetString(gameMode_key, instance.defaultGameMode);

			private set
			{
				if(GameMode == value)
					return;

				PlayerPrefs.SetString(gameMode_key, value);
				OnGameModeChange();
			}
		}

		static bool firstAwake = true;

		static GameModeManager instance;

		public static GameModeManager Instance
		{
			get
			{
				return instance;
			}
		}

		public bool IsDefaultGameMode
		{
			get
			{
				return GameMode == defaultGameMode;
			}
		}

		public bool IsGameModeEnabled(string gameModeToCheck)
		{
			return GameMode == gameModeToCheck;
		}

		public bool EnableGameMode(string gameModeToEnable)
		{
			if(GameMode == gameModeToEnable)
				return false;

			GameMode = gameModeToEnable;
			return true;
		}

		public void ReplaceAndSetDefaultGameMode(string newDefaultGameMode)
		{
			defaultGameMode = newDefaultGameMode;
			GameMode = newDefaultGameMode;
		}

		void Awake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}

			if(firstAwake)
			{
				firstAwake = false;
				FirstAwake();
			}

			OnGameModeChange();
		}

		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying == false)
			{
				instance = this;
			}
		}
		#endif

		void FirstAwake()
		{
			if(forceDefaultGameModeOnFirstAwake == false)
				return;

			GameMode = defaultGameMode;
		}

		void OnGameModeChange()
		{
			onGameModeChange?.Invoke();
		}
	}
}