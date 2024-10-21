using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_TextColor")]
	public class SkinItemApplicator_TextColor : SkinItemApplicator_ColorBase
	{
		Text textComponent;

		protected override Color CurrentColor
		{
			get
			{
				if(textComponent == null)
					textComponent = GetComponent<Text>();

				return textComponent.color;
			}
		}

		protected override void OnColorChange(Color color)
		{
			if(textComponent == null)
				textComponent = GetComponent<Text>();
			
			textComponent.color = color;
		}
	}
}
