using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_Texture")]
	public class ButtonRenderer_Texture : ButtonRenderer
	{	
		public Renderer spriteRenderer;
		
		public Texture spriteUp;
		
		public Texture spriteDown;
		
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
		
		void SetSprite(Texture a_rSprite)
		{
			spriteRenderer.material.mainTexture = a_rSprite;
		}
	}
}