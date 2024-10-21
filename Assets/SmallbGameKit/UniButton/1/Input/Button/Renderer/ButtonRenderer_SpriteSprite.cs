using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_SpriteSprite")]
	public class ButtonRenderer_SpriteSprite : ButtonRenderer_Sprite
	{	
		public SpriteRenderer spriteRenderer;
		
		protected override void SetSprite(Sprite sprite)
		{
			spriteRenderer.sprite = sprite;
		}
	}
}