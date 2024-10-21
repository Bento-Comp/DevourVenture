using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/ActivationController_BinaryActivator")]
	public class ActivationController_BinaryActivator : ActivationControllerBase
	{
		public int activeIndex = 1;

		public int inactiveIndex = 0;

		public Activator activator;

		int ActivationIndex => Active ? activeIndex : inactiveIndex;

		protected override void OnSetFirstActiveState()
		{
			activator.SetFirstActiveState(ActivationIndex);
		}

		protected override void OnActiveChange()
		{
			UpdateControlledObjectActivation();
		}

		protected override void OnDeactivationEnd()
		{
			UpdateControlledObjectActivation();
		}

		#if UNITY_EDITOR
		protected override void OnEditorActiveChange()
		{
			UpdateControlledObjectActivation();
		}
		#endif

		void OnEnable()
		{
			if(Active == false && FirstActive == false)
			{
				UpdateControlledObjectActivation();
			}
		}

		void UpdateControlledObjectActivation()
		{
			activator.SelectedIndex = ActivationIndex;
		}
	}
}