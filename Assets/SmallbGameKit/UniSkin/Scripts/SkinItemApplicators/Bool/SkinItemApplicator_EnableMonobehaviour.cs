using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_EnableMonobehaviour")]
	public class SkinItemApplicator_EnableMonobehaviour : SkinItemApplicator_BoolBase
	{
		public MonoBehaviour monobehaviour;
		
		protected override void OnBoolChange(bool value)
		{
			monobehaviour.enabled = value;
		}
	}
}
