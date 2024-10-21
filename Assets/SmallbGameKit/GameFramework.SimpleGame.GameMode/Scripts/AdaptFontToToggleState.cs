using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.SimpleGame.GameMode
{
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/AdaptFontToToggleState")]
	public class AdaptFontToToggleState : ToggleButton
	{
		public Font selectedFont;

		public Font notSelectedFont;

		public Text textComponent;

		public override void OnValueChange(bool value)
		{
			if(value)
			{
				textComponent.font = selectedFont;
			}
			else
			{
				textComponent.font = notSelectedFont;
			}
		}
	}
}
