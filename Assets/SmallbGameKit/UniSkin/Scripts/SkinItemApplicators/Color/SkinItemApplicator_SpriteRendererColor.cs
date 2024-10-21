using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_SpriteRendererColor")]
	public class SkinItemApplicator_SpriteRendererColor : SkinItemApplicator_ColorBase
	{
		SpriteRenderer sprite;

		protected override Color CurrentColor
		{
			get
			{
				if(sprite == null)
					sprite = GetComponent<SpriteRenderer>();

				return sprite.color;
			}
		}

		protected override void OnColorChange(Color color)
		{
			if(sprite == null)
				sprite = GetComponent<SpriteRenderer>();
			
			sprite.color = color;
		}
	}
}
