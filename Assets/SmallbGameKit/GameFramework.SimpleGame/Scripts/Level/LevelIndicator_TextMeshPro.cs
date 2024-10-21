using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using TMPro;

namespace GameFramework.SimpleGame
{
	[ExecuteAlways()]
	[AddComponentMenu("GameFramework/LevelIndicator_TextMeshPro")]
	public class LevelIndicator_TextMeshPro : LevelIndicatorBase 
	{
		public TextMeshProUGUI textComponent;

		protected override void SetText(string text)
		{
			textComponent.text = text;
		}
	}
}