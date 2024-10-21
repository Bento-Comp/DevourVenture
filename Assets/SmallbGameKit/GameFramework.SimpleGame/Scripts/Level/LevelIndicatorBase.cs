using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/LevelIndicatorBase")]
	public abstract class LevelIndicatorBase : MonoBehaviour 
	{
		public string prefix = "Level ";

		public bool noTextForFirstLevel;

		bool started;

		protected abstract void SetText(string text);

		void OnEnable()
		{
			if(started == false)
				return;
			
			UpdateLevelIndicator();
		}

		void Start()
		{
			started = true;

			UpdateLevelIndicator();
		}

#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			UpdateLevelIndicator();
		}
#endif

		void UpdateLevelIndicator()
		{
			int levelIndex = LevelManager.LevelIndex_RawAndContinuous;

			string text;
			if(noTextForFirstLevel && levelIndex <= 1)
			{
				text = "";
			}
			else
			{
				text = prefix + LevelManager.LevelIndex_RawAndContinuous.ToString("00");
			}

			SetText(text);
		}
	}
}