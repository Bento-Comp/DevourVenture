using UnityEngine;

using UniGameMode;

using UniActivation;

namespace GameFramework.SimpleGame.GameMode
{
    [DefaultExecutionOrder(1)]
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/ActivateGameModeNotification")]
	public class ActivateGameModeNotification : MonoBehaviour
	{
		public GameModeRadioButton gameModeRadioButton;

		public Activator activator;

		public int gamePlayedCountBeforeNotification = 3;

		bool firstUpdateAfterEnable;

		bool gameHasBeenCounted;

		static string playCount_keySave_Prefix = "ActivateModeNotification_PlayCount_";
		string PlayCount_keySave => playCount_keySave_Prefix + gameModeRadioButton.gameMode;
		int PlayCount
		{
			get
			{
				return PlayerPrefs.GetInt(PlayCount_keySave, 0);
			}

			set
			{
				PlayerPrefs.SetInt(PlayCount_keySave, value);
			}
		}

		static string hasBeenPlayed_keySave_Prefix = "ActivateModeNotification_HasBeenPlayed_";
		string HasBeenPlayed_keySave => hasBeenPlayed_keySave_Prefix + gameModeRadioButton.gameMode;
		bool HasBeenPlayed
		{
			get
			{
				return PlayerPrefs.GetInt(HasBeenPlayed_keySave, 0) == 1;
			}

			set
			{
				PlayerPrefs.SetInt(HasBeenPlayed_keySave, value?1:0);
			}
		}

		bool IsGameModeEnabled => GameModeManager.Instance.IsGameModeEnabled(gameModeRadioButton.gameMode);

		bool MustNotify
		{
			get
			{
				return PlayCount >= gamePlayedCountBeforeNotification && HasBeenPlayed == false
					&& IsGameModeEnabled == false;
			}
		}

		void OnEnable()
		{
			firstUpdateAfterEnable = true;
		}

		void Start()
		{
			ContinueManager.onGameOverConfirmed += OnGameOverConfirmed;

			GameModeManager.onGameModeChange += OnGameModeChange;

			Game.onLevelCompleted += OnLevelCompleted;
		}

		void OnDestroy()
		{
			ContinueManager.onGameOverConfirmed -= OnGameOverConfirmed;

			GameModeManager.onGameModeChange -= OnGameModeChange;

			Game.onLevelCompleted -= OnLevelCompleted;
		}

		void LateUpdate()
		{
			if(firstUpdateAfterEnable)
			{
				firstUpdateAfterEnable = false;
				UpdateState();
			}
		}

		void OnGameOverConfirmed()
		{
			OnGamePlayed();
		}

		void OnLevelCompleted(bool success)
		{
			OnGamePlayed();
		}

		void OnGamePlayed()
		{
			if(gameHasBeenCounted)
				return;

			gameHasBeenCounted = true;

			int gamePlayedCount = PlayCount;
			++gamePlayedCount;
			PlayCount = gamePlayedCount;

			if(IsGameModeEnabled)
				HasBeenPlayed = true;
		}

		void OnGameModeChange()
		{
			UpdateState();
		}

		void UpdateState()
		{
			bool mustNotify = MustNotify;
			activator.SelectedIndex = mustNotify ? 1 : 0;
		}
	}
}