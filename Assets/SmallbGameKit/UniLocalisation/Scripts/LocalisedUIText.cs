using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using UnityEngine.UI;

namespace UniLocalisation
{
	[AddComponentMenu("UniLocalisation/LocalisedUIText")]
	public class LocalisedUIText : LocalisedTextBase
	{	
		Text text;

		protected override void FetchComponent()
		{
			text = GetComponent<Text>();
		}

		protected override void SetText(string text)
		{
			this.text.text = text;
		}
	}
}