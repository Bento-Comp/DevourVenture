using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_Sprite")]
	public abstract class ButtonRenderer_Sprite : ButtonRenderer
	{	
		public Sprite spriteUp;
		
		public Sprite spriteDown;

		protected abstract void SetSprite(Sprite sprite);
		
		protected override void SetUp()
		{
			SetSprite(spriteUp);
		}
		
		protected override void SetDown()
		{
			SetSprite(spriteDown);
		}
	}
}