using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[ExecuteAlways()]
	[AddComponentMenu("GameFramework/LevelIndicator")]
	public class LevelIndicator : LevelIndicatorBase 
	{
		public Text textComponent;

		protected override void SetText(string text)
		{
			textComponent.text = text;
		}
	}
}