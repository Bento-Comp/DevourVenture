using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UniActivation;

namespace GameFramework.SimpleGame
{
	[ExecuteAlways()]
	[AddComponentMenu("GameFramework/SimpleGame/LevelingActivator")]
	public class LevelingActivator : MonoBehaviour
	{
		public  Activator activator;

		int ActivationIndex => LevelManager.UseLeveling?0:1;

		void Awake()
		{
			LevelManager.onUseLevelingChange += OnUseLevelingChange;
			activator.SetFirstActiveState(ActivationIndex);
		}

		void OnDestroy()
		{
			LevelManager.onUseLevelingChange -= OnUseLevelingChange;
		}

		void OnUseLevelingChange()
		{
			activator.SelectedIndex = ActivationIndex;
		}

#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			OnUseLevelingChange();
		}
#endif
	}
}
