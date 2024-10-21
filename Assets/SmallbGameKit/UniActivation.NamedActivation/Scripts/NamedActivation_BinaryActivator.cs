using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniActivation
{
	[ExecuteInEditMode()]
	[DefaultExecutionOrder(-31997)]
	[AddComponentMenu("UniActivation/NamedActivation_BinaryActivator")]
	public class NamedActivation_BinaryActivator : NamedActivation_ActivatorBase
	{
		[SerializeField]
		string activationName = "";

		protected override int ComputeActivationIndex()
		{
			bool isActive = register.IsActive(activationName);

			int selectedIndex = isActive?1:0;

			return selectedIndex;
		}
	}
}