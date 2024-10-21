using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using UnityEngine.UI;

using TMPro;

namespace UniLocalisation
{
	[AddComponentMenu("UniLocalisation/LocalisedTextMeshProUGUI")]
	public class LocalisedTextMeshProUGUI : LocalisedTextBase
	{	
		TextMeshProUGUI text;

		protected override void FetchComponent()
		{
			text = GetComponent<TextMeshProUGUI>();
		}

		protected override void SetText(string text)
		{
			this.text.text = text;
		}
	}
}