using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace UniLocalisation
{
	[AddComponentMenu("UniLocalisation/LocalisedTextMeshFont")]
	public class LocalisedTextMeshFont : MonoBehaviour
	{	
		TextMesh textMesh;
		
		TextMeshFontParameters initialTextMeshFontParameters = new TextMeshFontParameters();
		
		void Awake()
		{
			textMesh = GetComponent<TextMesh>();
			initialTextMeshFontParameters.CopyFrom(textMesh);
			UpdateFont();
		}
		
		void UpdateFont()
		{
			FontLocalisation.ApplyFontLocalisation(textMesh, initialTextMeshFontParameters);
		}
	}
}