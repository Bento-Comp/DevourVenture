﻿using System;
using UnityEngine;

using GameFramework;

namespace UniSingleton
{
	[DefaultExecutionOrder(-32000)]
	public abstract class GameBehaviourSingleton<T> : GameBehaviour where T : MonoBehaviour
	{
		// Use this with caution:
		// useful when you need to switch
		// between multiple instances
		// (ex: for Game Mode or AB tests)
		public bool canSwitchInstanceAtRuntime;

		public static T Instance {get;private set;}

		protected virtual void OnSingletonEnable(){}

		#if UNITY_EDITOR
		protected virtual void Editor_OnSingletonLateUpdate(){}
		#endif

		protected override void OnEnable()
		{
			base.OnEnable();

			if(Instance == null || canSwitchInstanceAtRuntime)
			{
				Instance = this as T;

				OnSingletonEnable();
			}
			else
			{
				#if UNITY_EDITOR
				if(Application.isPlaying == false)
					return;
				#endif

				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}
		
		protected override void OnDisable()
		{
			base.OnDisable();

			if(Instance == this)
			{
				Instance = null;
			}
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying == false)
			{
				Instance = this as T;
			}

			Editor_OnSingletonLateUpdate();
		}
		#endif
	}
}