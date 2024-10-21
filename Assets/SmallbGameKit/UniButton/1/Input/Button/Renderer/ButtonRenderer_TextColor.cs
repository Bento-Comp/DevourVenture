using UnityEngine;
using System.Collections;
using System;

using UnityEngine.UI;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_TextColor")]
	public class ButtonRenderer_TextColor : ButtonRenderer_Color
	{	
		public Text text;

		protected override Color GetColor()
		{
			return text.color;
		}

		protected override void SetColor(Color color)
		{
			text.color = color;
		}
	}
}