using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_ImageColor")]
	public class SkinItemApplicator_ImageColor : SkinItemApplicator_ColorBase
	{
		Image image;

		protected override Color CurrentColor
		{
			get
			{
				if(image == null)
				image = GetComponent<Image>();

				return image.color;
			}
		}

		protected override void OnColorChange(Color color)
		{
			if(image == null)
				image = GetComponent<Image>();
			
			image.color = color;
		}
	}
}
