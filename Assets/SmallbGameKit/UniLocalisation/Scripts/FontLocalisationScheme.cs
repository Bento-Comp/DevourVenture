using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



namespace UniLocalisation
{
	[Serializable]
	public class LanguageFont
	{
		public Font font;
		public float characterScaleOffset = 0.0f;
		public ETextCase textCase = ETextCase.Free;
		public FontStyle fontStyle;
	}

	[AddComponentMenu("UniLocalisation/Localisation/FontLocalisationScheme")]
	public class FontLocalisationScheme : MonoBehaviour
	{
		public List<LanguageFont> fonts;
	}
}