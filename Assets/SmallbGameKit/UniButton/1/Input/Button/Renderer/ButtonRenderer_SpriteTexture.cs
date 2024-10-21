using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_SpriteTexture")]
	public class ButtonRenderer_SpriteTexture : ButtonRenderer
	{	
		public SpriteRenderer spriteRenderer;
		
		public Sprite spriteUp;
		
		public Sprite spriteDown;
		
		void Start()
		{
			if(spriteRenderer == null)
			{
				spriteRenderer = GetComponent<SpriteRenderer>();
			}
		}
		
		protected override void SetUp()
		{
			SetSprite(spriteUp);
		}
		
		protected override void SetDown()
		{
			SetSprite(spriteDown);
		}
		
		void SetSprite(Sprite a_rSprite)
		{
			spriteRenderer.sprite = a_rSprite;
		}
	}
}