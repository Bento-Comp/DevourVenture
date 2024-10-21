using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UniActivation;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(1)]
	[ExecuteInEditMode()]
	[AddComponentMenu("GameFramework/SimpleGame/LevelActivator")]
	public class LevelActivator : MonoBehaviour
	{
		public  Activator activator;

		public bool onlySetOnStart = true;

		[Header("Editor")]
		public bool editor_autoFill = true;

		void Awake()
		{
			ApplyCurrentLevel();
		}

		void OnEnable()
		{
			LevelManager.onLevelChange += OnLevelChange;
		}

		void OnDisable()
		{
			LevelManager.onLevelChange -= OnLevelChange;
		}

		void OnLevelChange()
		{
			if(Application.isPlaying && onlySetOnStart)
				return;

			if(isActiveAndEnabled == false)
				return;

			ApplyCurrentLevel();
		}

		void ApplyCurrentLevel()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				if(activator == null)
					return;
			}
			#endif

			activator.SetFirstActiveState(LevelManager.LevelIndex_LoopedAndSkipped - 1);
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			if(editor_autoFill)
			{
				AutoFill();
			}
		}

		void AutoFill()
		{
			if(activator == null)
			{
				activator = gameObject.GetComponent<Activator>();
				if(activator == null)
					activator = gameObject.AddComponent<Activator>();
			}

			List<ActivationGroup> activationGroups = activator.activationGroups;

			activationGroups.Clear();

			foreach(Transform child in transform)
			{
				ActivationGroup activationGroup = new ActivationGroup();

				activationGroup.gameObjects = new GameObject[]{child.gameObject};

				activationGroups.Add(activationGroup);
			}

			activator.activationGroups = activationGroups;
		}
		#endif
	}
}
