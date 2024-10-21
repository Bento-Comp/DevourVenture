using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine.EventSystems;

namespace GameFramework
{
	[AddComponentMenu("GameFramework/GameScreenButton")]
	public class GameScreenButton : MonoBehaviour 
	{
		public static System.Action onDown;

		public static System.Action onDownDuringGamePlay;

		public static System.Action onUp;

		public static System.Action onUpDuringGamePlay;

		public bool levelCompletedScreenAfterGameOver;

		public bool levelCompletedScreenAfterGameOver_IsSuccess;

		public bool handleRestart;

		public bool clickToStart;

		public bool handleInterludeEnd = true;

		public bool clickToEndInterlude;

		public UniHapticFeedback.EHapticFeedbackType haptic_restart;

		static GameScreenButton instance;

		bool pressed;

		bool ignoreNextDown;

		bool restartButtonDown;

		bool startButtonDown;

		bool endInterludeButtonDown;

		public static GameScreenButton Instance
		{
			get
			{
				return instance;
			}
		}

		public bool Pressed
		{
			get
			{
				return pressed;
			}
		}

		public void OnPointerDown(BaseEventData eventData)
		{
			pressed = true;
			TriggerDown();
		}

		public void TriggerDown()
		{
			if(ignoreNextDown)
			{
				ignoreNextDown = false;
				return;
			}

			if(Game.Instance.IsGamePlay)
				if(onDownDuringGamePlay != null)
					onDownDuringGamePlay();

			if(handleRestart && Game.Instance.CanRestart)
			{
				restartButtonDown = true;
			}

			if(handleInterludeEnd && Game.Instance.CanEndInterlude)
			{
				if(clickToEndInterlude)
				{
					endInterludeButtonDown = true;
				}
				else
				{
					if(Game.Instance.CanEndInterlude)
					{
						Game.Instance.InterludeEnd();
					}
				}
			}

			if(clickToStart)
			{
				startButtonDown = true;
			}
			else
			{
				#if UNITY_EDITOR
				if(Game.Instance.debug_levelCompleteAtStart)
				{
					Game.Instance.LevelCompleted(true);
					return;
				}
				#endif
				if(Game.Instance.IsCover)
				{
					Game.Instance.GameStart();
				}
			}

			if(onDown != null)
				onDown();
		}

		public void OnPointerUp(BaseEventData eventData)
		{
			ignoreNextDown = false;
			pressed = false;
			TriggerUp();
		}

		public void TriggerUp()
		{
			if(Game.Instance.IsGamePlay)
			if(onUpDuringGamePlay != null)
				onUpDuringGamePlay();

			if(restartButtonDown && handleRestart && Game.Instance.CanRestart)
			{
				if(Game.Instance.IsGameOver && levelCompletedScreenAfterGameOver
					&& Game.Instance.IsLevelCompleted == false)
				{
					Game.Instance.LevelCompleted(levelCompletedScreenAfterGameOver_IsSuccess);
				}
				else
				{
					Game.Instance.AskForRestart();
					UniHapticFeedback.HapticFeedbackManager.TriggerHapticFeedback(haptic_restart);
				}
			}

			if(clickToStart && startButtonDown)
			{
				if(Game.Instance.IsCover)
				{
					Game.Instance.GameStart();
				}
			}

			if(endInterludeButtonDown && handleInterludeEnd && Game.Instance.CanEndInterlude)
			{
				Game.Instance.InterludeEnd();
			}

			if(onUp != null)
				onUp();

			startButtonDown = false;

			restartButtonDown = false;

			endInterludeButtonDown = false;
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

			EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

			EventTrigger.Entry pointerDown = new EventTrigger.Entry();
			pointerDown.eventID = EventTriggerType.PointerDown;
			pointerDown.callback.AddListener(OnPointerDown);
			trigger.triggers.Add(pointerDown);

			EventTrigger.Entry pointerUp = new EventTrigger.Entry();
			pointerUp.eventID = EventTriggerType.PointerUp;
			pointerUp.callback.AddListener(OnPointerUp);
			trigger.triggers.Add(pointerUp);

			if(Input.GetMouseButton(0))
				ignoreNextDown = true;
		}

		void Update()
		{
			if(Input.GetMouseButton(0) == false)
				ignoreNextDown = false;
		}

		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}
	}
}
