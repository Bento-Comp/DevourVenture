using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniLocalisation;

namespace UniSkin.Localisation
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniSkin/SkinItemApplicator_LocalisedStringBase")]
	public abstract class SkinItemApplicator_LocalisedStringBase : SkinItemApplicator_StringBase
	{
		public string localisationItemIDFormat = "XXX";

		protected override void OnStringChange(string value)
		{
			OnLocalisedStringChange(UniLocalisation.Localisation.GetString(localisationItemIDFormat.Replace("XXX", value)));
		}

		protected virtual void OnLocalisedStringChange(string value)
		{
		}
	}
}
