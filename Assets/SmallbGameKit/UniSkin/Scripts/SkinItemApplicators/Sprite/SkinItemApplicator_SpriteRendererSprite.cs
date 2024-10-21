using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_SpriteRendererSprite")]
	public class SkinItemApplicator_SpriteRendererSprite : SkinItemApplicator_SpriteBase
	{
		SpriteRenderer spriteRenderer;

		protected override void OnSpriteChange(Sprite sprite)
		{
			if(spriteRenderer == null)
				spriteRenderer = GetComponent<SpriteRenderer>();
			
			spriteRenderer.sprite = sprite;
		}
	}
}
