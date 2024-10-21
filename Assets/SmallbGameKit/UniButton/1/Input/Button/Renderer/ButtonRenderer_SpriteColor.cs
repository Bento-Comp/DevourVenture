using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_SpriteColor")]
	public class ButtonRenderer_SpriteColor : ButtonRenderer_Color
	{	
		public SpriteRenderer spriteRenderer;
		
		protected override Color GetColor()
		{
			return spriteRenderer.color;
		}

		protected override void SetColor(Color color)
		{
			spriteRenderer.color = color;
		}
	}
}