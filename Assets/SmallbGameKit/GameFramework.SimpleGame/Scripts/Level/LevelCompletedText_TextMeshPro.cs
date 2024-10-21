using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using TMPro;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/LevelCompletedText_TextMeshPro")]
	public class LevelCompletedText_TextMeshPro : LevelCompletedTextBase
	{	
		public TextMeshProUGUI text;

		protected override void SetText(string text)
		{
			this.text.text = text;
		}
	}
}