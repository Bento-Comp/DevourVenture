using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GameFramework
{
	[DefaultExecutionOrder(-32000)]
	[AddComponentMenu("GameFramework/Game")]
	public class Game : MonoBehaviour 
	{
		public static System.Action onLoadGame;

		public static System.Action<bool> onLoadGameEnd;

		public static System.Action onGameStart;

		public static System.Action onInterlude;

		public static System.Action onInterludeEnd;

		public static System.Action onGameOver;

		public static System.Action<bool> onLevelCompleted;

		public static System.Action<bool> onLevelCompletedEnd;

		public static  System.Action onGameStateChange;

		public static System.Action onSkipLevel;

		public bool immediateStart;

		public float delayBeforeCanRestart = 0.1f;

		public float delayBeforeCanLaunchNextLevel = 0.1f;

		public float delayBeforeCanEndInterlude = 0.1f;

		public EGameRestartMode restartMode = EGameRestartMode.HardSceneReload;

		public Game_CustomRestarter_Base customRestarter;

		[Header("Debug")]
		public bool debug_usePlayTimeOffset;
		public float debug_playTimeOffset;

		public bool debug_levelCompleteAtStart;
		public bool debug_logEnabled;


		List<IGameBehaviour> gameBehaviours = new List<IGameBehaviour>();

		bool loopingThroughGameBehaviours;

		List<IGameBehaviour> gamebehavioursToUnregister = new List<IGameBehaviour>();
		List<IGameBehaviour> gamebehavioursToRegister = new List<IGameBehaviour>();

		static bool firstGameLoadEndStatic = false;

		static bool firstGameLoadStatic = true;

		bool firstGameLoadEnd = false;

		bool firstGameLoad = true;

		bool gameLoaded;

		bool gameLoadedAtLeastOnce;

		bool gameStarted;

		bool interludeInProgress;

		bool gameOver;

		bool levelCompleted;

		bool levelSuccess;

		float playTime;

		float timeSinceInterludeStarted;

		float timeSinceGameOver;

		float timeSinceLevelCompleted;

		static Game instance;

		public static Game Instance
		{
			get
			{
				return instance;
			}
		}

		static bool Debug_LogEnabled
		{
			get
			{
				if(instance == null)
					return false;

				return instance.isActiveAndEnabled && instance.debug_logEnabled;
			}
		}

		public bool IsGamePlayOrCover
		{
			get => State == EGameState.Cover || State == EGameState.Play;
		}

		public bool IsFirstGameLoadStatic
		{
			get
			{
				return firstGameLoadStatic;
			}
		}

		public bool IsFirstGameLoad
		{
			get
			{
				return firstGameLoad;
			}
		}

		public bool IsGameLoadedAtLeastOnce
		{
			get
			{
				return gameLoadedAtLeastOnce;
			}
		}

		public bool IsGamePlay
		{
			get
			{
				return State == EGameState.Play;
			}
		}

		public bool IsGamePlayOrLevelCompleted
		{
			get
			{
				return State == EGameState.Play || State == EGameState.LevelCompleted;
			}
		}

		public bool IsCover
		{
			get
			{
				return State == EGameState.Cover;
			}
		}

		public bool IsGameStarted
		{
			get
			{
				return gameStarted;
			}
		}

		public bool IsInterludeInProgress => interludeInProgress;

		public bool IsGameOverOrLevelCompleted
		{
			get
			{
				return gameOver || levelCompleted;
			}
		}

		public bool IsGameOver
		{
			get
			{
				return gameOver;
			}
		}

		public bool IsLevelCompleted
		{
			get
			{
				return levelCompleted;
			}
		}

		public bool IsLevelSuccess
		{
			get
			{
				return levelSuccess;
			}
		}

		public bool CanRestart
		{
			get
			{
				return IsGameOver && timeSinceGameOver >= delayBeforeCanRestart
					|| IsLevelCompleted && timeSinceLevelCompleted >= delayBeforeCanRestart;
			}
		}

		public bool CanEndInterlude => IsInterludeInProgress && timeSinceInterludeStarted >= delayBeforeCanEndInterlude;

		public EGameState State
		{
			get
			{
				if(levelCompleted)
				{ 
					return EGameState.LevelCompleted;
				}
				else if(gameOver)
				{
					return EGameState.GameOver;
				}
				else if(interludeInProgress)
				{
					return EGameState.Interlude;
				}
				else if(gameStarted)
				{
					return EGameState.Play;
				}
				else
				{
					return EGameState.Cover;
				}
			}
		}

		public float PlayTime
		{
			get
			{
#if UNITY_EDITOR || debugGameFramework
				if(debug_usePlayTimeOffset)
				{
					return playTime + debug_playTimeOffset;
				}
#endif
				return playTime;
			}
		}

		static string gameCount_keySave = "GameFramework_GameCount";
		public int GameCount
		{
			get => PlayerPrefs.GetInt(gameCount_keySave, 0);
			set => PlayerPrefs.SetInt(gameCount_keySave, value);
		}

		public static void Log(string message)
		{
#if UNITY_EDITOR
			if(Debug_LogEnabled == false)
				return;

			Debug.Log("GameFramework : " + message);
#endif
		}

		public void GameStart()
		{
			if(gameStarted)
				return;

			++GameCount;

			Game.Log("Game : GameStart");

			gameStarted = true;

			loopingThroughGameBehaviours = true;
			foreach(IGameBehaviour gameBehaviour in gameBehaviours)
				gameBehaviour.NotifyGameStart();
			loopingThroughGameBehaviours = false;
			DelayedRegister();

			if(onGameStart != null)
				onGameStart();
			
			OnGameStateChange();
		}

		public void Interlude()
		{
			if(levelCompleted)
				return;

			if(gameOver)
				return;

			if(interludeInProgress)
				return;

			Game.Log("Game : Interlude");

			interludeInProgress = true;

			loopingThroughGameBehaviours = true;
			foreach(IGameBehaviour gameBehaviour in gameBehaviours)
				gameBehaviour.NotifyInterlude();
			loopingThroughGameBehaviours = false;
			DelayedRegister();

			if(onInterlude != null)
				onInterlude();

			OnGameStateChange();
		}

		public void InterludeEnd()
		{
			if(levelCompleted)
				return;

			if(gameOver)
				return;

			if(interludeInProgress == false)
				return;

			Game.Log("Game : Interlude End");

			interludeInProgress = false;
			timeSinceInterludeStarted = 0.0f;

			loopingThroughGameBehaviours = true;
			foreach(IGameBehaviour gameBehaviour in gameBehaviours)
				gameBehaviour.NotifyInterludeEnd();
			loopingThroughGameBehaviours = false;
			DelayedRegister();

			if(onInterludeEnd != null)
				onInterludeEnd();

			OnGameStateChange();
		}

		public void GameOver()
		{
			if(levelCompleted)
				return;

			if(gameOver)
				return;
		
			Game.Log("Game : GameOver");
			
			gameOver = true;

			loopingThroughGameBehaviours = true;
			foreach(IGameBehaviour gameBehaviour in gameBehaviours)
				gameBehaviour.NotifyGameOver();
			loopingThroughGameBehaviours = false;
			DelayedRegister();

			if(onGameOver != null)
				onGameOver();

			OnGameStateChange();
		}

		public void CancelGameOver()
		{
			if(gameOver == false)
				return;

			gameOver = false;
			timeSinceGameOver = 0.0f;

			OnGameStateChange();
		}

		public void SkipCurrentLevel()
		{
			if(onSkipLevel != null)
			{
				onSkipLevel();
			}

			LevelCompleted(true);
		}

		public void LevelCompleted(bool success)
		{
			if(levelCompleted)
				return;
		
			Game.Log("Game : LevelCompleted : success = " + success);
			
			levelCompleted = true;

			levelSuccess = success;

			loopingThroughGameBehaviours = true;
			foreach(IGameBehaviour gameBehaviour in gameBehaviours)
				gameBehaviour.NotifyLevelCompleted(success);
			loopingThroughGameBehaviours = false;
			DelayedRegister();

			if(onLevelCompleted != null)
				onLevelCompleted(success);

			OnGameStateChange();

			if(onLevelCompletedEnd != null)
				onLevelCompletedEnd(success);
		}

		public void Register(IGameBehaviour gameBehaviour)
		{
			if(loopingThroughGameBehaviours)
			{
				gamebehavioursToRegister.Add(gameBehaviour);
				return;
			}

			gameBehaviours.Add(gameBehaviour);
		}

		public void Unregister(IGameBehaviour gameBehaviour)
		{
			if(loopingThroughGameBehaviours)
			{
				gamebehavioursToUnregister.Add(gameBehaviour);
				return;
			}

			gameBehaviours.Remove(gameBehaviour);
		}

		public void CallFirstLoadGame()
		{	
			if(gameLoadedAtLeastOnce == false)
				LoadGame();
		}

		public void AskForRestart()
		{
			if(customRestarter != null)
			{
				customRestarter.AskForRestart();
				return;
			}

			DoRestart();
		}

		public void DoRestart()
		{
			switch(restartMode)
			{
				case EGameRestartMode.HardSceneReload:
				{	
					LoadGameHard();
				}
				break;

				case EGameRestartMode.ManagedRestart:
				{	
					LoadGame();
				}
				break;
			}
		}

		void OnApplicationQuit()
		{
			LoadGameEnd(false);
		}

		void Awake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}
		
		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}

		void Update()
		{
			if(State == EGameState.Play)
			{
				playTime += Time.deltaTime;
			}

			if(interludeInProgress)
			{
				timeSinceInterludeStarted += Time.deltaTime;
			}

			if(gameOver)
			{
				timeSinceGameOver += Time.deltaTime;
			}
			
			if(levelCompleted)
			{
				timeSinceLevelCompleted += Time.deltaTime;
			}
		}

		void LoadGameHard()
		{
			if(gameLoaded)
			{
				LoadGameEnd(true);
			}

			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		void LoadGame()
		{
			if(gameLoaded)
			{
				LoadGameEnd(false);
			}

			timeSinceInterludeStarted = 0.0f;

			timeSinceGameOver = 0.0f;

			timeSinceLevelCompleted = 0.0f;

			playTime = 0.0f;

			gameLoaded = true;

			if(firstGameLoadEnd)
			{
				firstGameLoad = false;
			}

			if(firstGameLoadEndStatic)
			{
				firstGameLoadStatic = false;
			}

			gameOver = false;
			levelCompleted = false;
			gameLoadedAtLeastOnce = true;

			loopingThroughGameBehaviours = true;
			foreach(IGameBehaviour gameBehaviour in gameBehaviours)
				gameBehaviour.NotifyLoadGame();
			loopingThroughGameBehaviours = false;
			DelayedRegister();

			if(onLoadGame != null)
			{
				onLoadGame();
			}
				
			firstGameLoadEnd = true;
			firstGameLoadEndStatic = true;

			if(immediateStart)
				GameStart();

			OnGameStateChange();
		}

		void LoadGameEnd(bool reloadSceneAfter)
		{
			gameLoaded = false;
			gameStarted = false;

			loopingThroughGameBehaviours = true;
			foreach(IGameBehaviour gameBehaviour in gameBehaviours)
				gameBehaviour.NotifyLoadGameEnd(reloadSceneAfter);
			loopingThroughGameBehaviours = false;
			DelayedRegister();

			if(onLoadGameEnd != null)
			{
				onLoadGameEnd(reloadSceneAfter);
			}
		}

		void DelayedRegister()
		{
			foreach(IGameBehaviour gameBehaviour in gamebehavioursToRegister)
			{
				gameBehaviours.Add(gameBehaviour);
			}
			gamebehavioursToRegister.Clear();

			foreach(IGameBehaviour gameBehaviour in gamebehavioursToUnregister)
			{
				gameBehaviours.Remove(gameBehaviour);
			}
			gamebehavioursToUnregister.Clear();
		}

		void OnGameStateChange()
		{
			if(onGameStateChange != null)
				onGameStateChange();
		}
	}
}
