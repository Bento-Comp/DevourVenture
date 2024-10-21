using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
	[AddComponentMenu("GameFramework/GameBehaviour")]
	public abstract class GameBehaviour : MonoBehaviour, IGameBehaviour
	{
		bool awaken;

		bool started;
		
		bool gameStartedBeforeLoad;

		bool firstGameLoad = true;

		bool haveBehaviourKnownAGameLoadAtLeastOnce;

		bool gameLoaded;

		bool gameStarted;

		public bool FirstGameLoad
		{
			get
			{
				return firstGameLoad;
			}
		}

		public bool HaveLevelStartAtLeastOnce
		{
			get
			{
				return haveBehaviourKnownAGameLoadAtLeastOnce;
			}
		}

		public bool Started
		{
			get
			{
				return started;
			}
		}

		public bool Awaken
		{
			get
			{
				return awaken;
			}
		}

		protected Game Game
		{
			get
			{
				return Game.Instance;
			}
		}

		public void NotifyLoadGame()
		{
			gameStarted = false;
			DoLoadGame();
		}

		public void NotifyLoadGameEnd(bool reloadSceneAfter)
		{
			DoLoadGameEnd(reloadSceneAfter);
		}

		public void NotifyGameStart()
		{
			DoGameStart();
		}

		public void NotifyInterlude()
		{
			OnInterlude();
		}

		public void NotifyInterludeEnd()
		{
			OnInterludeEnd();
		}

		public void NotifyGameOver()
		{
			OnGameOver();
		}

		public void NotifyLevelCompleted(bool success)
		{
			OnLevelCompleted(success);
		}

		protected virtual void OnLoadGame()
		{
		}

		protected virtual void OnLoadGameEnd(bool reloadSceneAfter)
		{
		}

		protected virtual void OnGameStart()
		{
		}

		protected virtual void OnInterlude()
		{
		}

		protected virtual void OnInterludeEnd()
		{
		}

		protected virtual void OnGameOver()
		{
		}

		protected virtual void OnLevelCompleted(bool success)
		{
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnAwakeEnd()
		{
		}

		protected virtual void OnStart()
		{
		}
		
		protected virtual void OnStartEnd()
		{
		}

		protected virtual void Awake()
		{			
			if(awaken)
				return;

			OnAwake();
			awaken = true;
		}

		protected virtual void Start()
		{
			if(started)
				return;

			OnStart();
			started = true;

			LateGameStartIfNeeded();
		}
		
		protected virtual void OnDestroy()
		{
			if(started)
			{
				OnStartEnd();
				started = false;
			}

			if(awaken)
			{
				OnAwakeEnd();
				awaken = false;
			}
		}

		protected virtual void OnEnable()
		{
			if(Application.isPlaying)
			{
				//Debug.Log("Register : " + this);
				if(Game != null)
					Game.Register(this);
			}

			LateGameLoadIfNeeded();
			LateGameStartIfNeeded();
		}

		protected virtual void OnDisable()
		{
			if(Game != null)
			{
				Game.Unregister(this);
			}
		}

		void DoLoadGame()
		{
			if(this == null)
				return;
			
			if(gameLoaded)
				return;
			
			if(isActiveAndEnabled || haveBehaviourKnownAGameLoadAtLeastOnce)
			{
				gameLoaded = true;

				////Debug.Log("Load Game : " + this);
				OnLoadGame();
				firstGameLoad = false;
				haveBehaviourKnownAGameLoadAtLeastOnce = true;

				if(gameStartedBeforeLoad)
				{
					DoGameStart();
				}
			}
		}

		void DoLoadGameEnd(bool reloadSceneAfter)
		{
			if(this == null)
				return;
			
			if(gameLoaded == false)
				return;

			gameLoaded = false;

			gameStartedBeforeLoad = false;
			
			OnLoadGameEnd(reloadSceneAfter);
		}

		void DoGameStart()
		{
			if(this == null)
				return;

			if(gameStarted)
				return;

			gameStarted = true;

			if(gameLoaded)
			{
				OnGameStart();
			}
			else
			{
				gameStartedBeforeLoad = true;
			}
		}

		void LateGameLoadIfNeeded()
		{
			if(Application.isPlaying)
			{
				if(Game != null && Game.IsGameLoadedAtLeastOnce)
				{
					DoLoadGame();
				}
			}
		}

		void LateGameStartIfNeeded()
		{
			if(Application.isPlaying)
			{
				if(Game != null && Game.IsGameStarted)
				{
					DoGameStart();
				}
			}
		}
	}
}