using UnityEngine;
using System.Collections;
using System;

using UnityEngine.UI;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_ImageSprite")]
	public class ButtonRenderer_ImageSprite : ButtonRenderer_Sprite
	{	
		public Image image;
		
		protected override void SetSprite(Sprite sprite)
		{
			image.sprite = sprite;
		}
	}
}