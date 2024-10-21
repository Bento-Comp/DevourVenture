using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.SimpleGame
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GameFramework/SimpleGame/DebugManager")]
	public class DebugManager : MonoBehaviour 
	{
		public static System.Action onDebugChange;

		public bool debugEnabled = true;

		public bool forceUseDebugInBuild = true;

		static DebugManager instance;

		bool debugWasEnabled = false;

		public static bool DebugEnabled
		{
			get
			{
				if(instance == null)
					return false;

				if(instance.isActiveAndEnabled == false)
					return false;

				if(instance.debugEnabled == false)
					return false;

				#if !UNITY_EDITOR
				if(instance.forceUseDebugInBuild == false)
					return false;
				#endif

				return true;
			}
		}
		
		void Awake()
		{
			if(instance == null)
			{
				instance = this;
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
		
		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}

		void Update()
		{
			if(debugWasEnabled != debugEnabled)
			{
				debugWasEnabled = debugEnabled;
				if(onDebugChange != null)
				{
					onDebugChange();
				}
			}
		}
	}
}
