using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using UnityEngine.UI;

namespace UniLocalisation
{
	[AddComponentMenu("UniLocalisation/LocalisedTextBase")]
	public abstract class LocalisedTextBase : MonoBehaviour
	{	
		public enum ECaseMode
		{
			None,
			ForceUpperCase,
			ForceLowerCase
		}
		public string prefix = "";
		
		public LocalisedString localisedText;
		
		public string suffixe = "";

		public ECaseMode caseMode;

		protected abstract void FetchComponent();

		protected abstract void SetText(string text);

		void Awake()
		{
			FetchComponent();
			UpdateText();
		}
		
		void UpdateText()
		{
			string text = prefix + localisedText.Value + suffixe;
			switch(caseMode)
			{
				case ECaseMode.ForceLowerCase:
				{
					text = text.ToLower();
				}
				break;

				case ECaseMode.ForceUpperCase:
				{
					text = text.ToUpper();
				}
				break;
			}
			SetText(text);
		}
	}
}