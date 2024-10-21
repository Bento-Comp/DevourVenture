using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[AddComponentMenu("UniActivation/ActivationController_Animator_Slave")]
	public class ActivationController_Animator_Slave : ActivationControllerBase
	{
		public ActivationControllerBase master;

		protected override void OnSetFirstActiveState()
		{
			OnActiveChange();
		}

		protected override void OnActiveChange()
		{
			if(Active)
			{
				gameObject.SetActive(true);
			}
			else
			{
				if(master.ActivationInProgress == false)
					gameObject.SetActive(false);
			}
		}

		#if UNITY_EDITOR
		protected override void OnEditorActiveChange()
		{
			gameObject.SetActive(Active);
		}
		#endif

		void Awake()
		{
			master.onActivationEnd += OnActivationEnd;
		}

		void OnDestroy()
		{
			master.onActivationEnd -= OnActivationEnd;
		}

		void OnActivationEnd(ActivationControllerBase master)
		{
			gameObject.SetActive(Active);
		}
	}
}