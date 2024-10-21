using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UniUI;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/LevelCompletedText")]
	public class LevelCompletedText : LevelCompletedTextBase
	{	
		public Text text;

		protected override void SetText(string text)
		{
			this.text.text = text;
		}
	}
}