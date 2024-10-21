using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[AddComponentMenu("UniActivation/ActivationController_Activator_Link")]
	public class ActivationController_Activator_Link : ActivationControllerBase
	{
		public Activator activator_master;

		public Activator activator_slave;

		protected override void OnSetFirstActiveState()
		{
			UpdateActivation();
		}

		protected override void OnActiveChange()
		{
			UpdateActivation();
		}

		#if UNITY_EDITOR
		protected override void OnEditorActiveChange()
		{
			UpdateActivation();
		}
		#endif

		void UpdateActivation()
		{
			activator_slave.SelectedIndex = activator_master.SelectedIndex;
		}
	}
}