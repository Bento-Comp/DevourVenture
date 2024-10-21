using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_ImageSprite")]
	public class SkinItemApplicator_ImageSprite : SkinItemApplicator_SpriteBase
	{
		Image image;

		protected override void OnSpriteChange(Sprite sprite)
		{
			if(image == null)
				image = GetComponent<Image>();
			
			image.sprite = sprite;
		}
	}
}
