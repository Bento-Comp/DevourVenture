using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniLocalisation;

namespace UniSkin.Localisation
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniSkin/SkinItemApplicator_LocalisedText")]
	public class SkinItemApplicator_LocalisedText : SkinItemApplicator_LocalisedStringBase
	{
		Text textComponent;
		protected override void OnLocalisedStringChange(string value)
		{
			if(textComponent == null)
				textComponent = GetComponent<Text>();

			textComponent.text = value;
		}
	}
}
