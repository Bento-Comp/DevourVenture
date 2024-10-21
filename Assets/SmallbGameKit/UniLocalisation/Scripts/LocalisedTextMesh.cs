using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace UniLocalisation
{
	[AddComponentMenu("UniLocalisation/LocalisedTextMesh")]
	public class LocalisedTextMesh : MonoBehaviour
	{	
		public string prefix = "";
		
		public LocalisedString localisedText;
		
		public string suffixe = "";

		public bool forceUpper;
		
		TextMesh textMesh;
		
		TextMeshFontParameters initialTextMeshFontParameters = new TextMeshFontParameters();
		
		void Awake()
		{
			textMesh = GetComponent<TextMesh>();
			initialTextMeshFontParameters.CopyFrom(textMesh);
			UpdateText();
		}
		
		void UpdateText()
		{
			string text = prefix + localisedText.Value + suffixe;
			if(forceUpper)
				text = text.ToUpper();
			textMesh.text = text;
			FontLocalisation.ApplyFontLocalisation(textMesh, initialTextMeshFontParameters);
		}
	}
}