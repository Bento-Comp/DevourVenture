using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UniUI;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/LevelCompletedTextBase")]
	public abstract class LevelCompletedTextBase : MonoBehaviour
	{	
		public string suffix = "Level ";

		public string prefix_success = " Completed!";

		public string prefix_failed = " Failed";

		public bool useEscapedLineReturn;

		protected abstract void SetText(string text);

		void OnEnable()
		{
			string levelCompletedText = suffix + LevelManager.LevelIndex_RawAndContinuous.ToString("00");

			if(Game.Instance.IsLevelSuccess)
			{
				levelCompletedText += prefix_success;
			}
			else
			{
				levelCompletedText += prefix_failed;
			}

			if(useEscapedLineReturn)
				levelCompletedText = levelCompletedText.Replace("\\n", "\n");

			SetText(levelCompletedText);
		}
	}
}