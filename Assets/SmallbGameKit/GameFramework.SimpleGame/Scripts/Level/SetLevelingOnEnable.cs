using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(-31998)]
	[ExecuteAlways()]
	[AddComponentMenu("GameFramework/SimpleGame/SetLevelingOnEnable")]
	public class SetLevelingOnEnable : MonoBehaviour
	{
		public bool leveling;

		bool awaken;

		void OnEnable()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			if(awaken == false)
				return;

			SetLeveling();
		}

		void Awake()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			awaken = true;

			SetLeveling();
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			SetLeveling();
		}
		#endif

		void SetLeveling()
		{
			LevelManager manager = LevelManager.Instance;

			manager.leveling = leveling;
		}
	}
}
