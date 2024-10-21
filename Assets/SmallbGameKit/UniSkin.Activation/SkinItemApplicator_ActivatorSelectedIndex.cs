using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniActivation;

namespace UniSkin.Localisation
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniSkin/SkinItemApplicator_ActivatorSelectedIndex")]
	public class SkinItemApplicator_ActivatorSelectedIndex : SkinItemApplicator_IntBase
	{
		Activator activator;

		protected override void OnIntChange(int value)
		{
			if(activator == null)
			{
				activator = GetComponent<Activator>();
			}

			activator.SelectedIndex = value;
		}
	}
}
