using UnityEngine;
using System.Collections;
using System;

using UnityEngine.UI;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_ImageColor")]
	public class ButtonRenderer_ImageColor : ButtonRenderer_Color
	{	
		public Image image;

		protected override Color GetColor()
		{
			return image.color;
		}

		protected override void SetColor(Color color)
		{
			image.color = color;
		}
	}
}