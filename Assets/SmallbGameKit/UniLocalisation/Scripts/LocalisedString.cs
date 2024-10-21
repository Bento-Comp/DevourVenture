using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace UniLocalisation
{
	[System.Serializable]
	public class LocalisedString
	{	
		[SerializeField]
		string localisationID = "";
		
		public string Value
		{
			get
			{
				return Localisation.GetString(localisationID);
			}	
		}
	}
}