using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniGameMode;

namespace GameFramework.SimpleGame.GameMode
{
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/RestartOnGameModeChange")]
	public class RestartOnGameModeChange : MonoBehaviour
	{
		void Awake()
		{
			GameModeManager.onGameModeChange += OnGameModeChange;
		}

		void OnDestroy()
		{
			GameModeManager.onGameModeChange -= OnGameModeChange;
		}

		void OnGameModeChange()
		{
			Game.Instance.AskForRestart();
		}
	}
}
