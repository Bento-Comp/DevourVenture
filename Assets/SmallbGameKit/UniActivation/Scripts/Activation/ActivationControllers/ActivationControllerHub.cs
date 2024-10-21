using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[AddComponentMenu("UniActivation/ActivationControllerHub")]
	public class ActivationControllerHub : ActivationControllerBase
	{
		public bool controlGameObjectActivation = true;

		public ActivationControllerBase[] controllers;

		int activatedChildrenCount;

		public void NotifyRuntimeChildDeactivationEnd()
		{
			--activatedChildrenCount;
			if(activatedChildrenCount <= 0)
			{
				NotifyRuntimeDeactivationEnd();
			}
		}

		protected override void OnSetFirstActiveState()
		{
			if(controlGameObjectActivation)
				gameObject.SetActive(Active);

			NotifyChildrenOfSetFirstActiveState();
		}

		protected override void OnActiveChange()
		{
			if(controlGameObjectActivation && Active)
				gameObject.SetActive(true);
			
			NotifyChildrenOfActiveChange();
		}

		protected override void OnDeactivationEnd()
		{
			if(controlGameObjectActivation)
				gameObject.SetActive(false);
		}

		#if UNITY_EDITOR
		protected override void OnEditorActiveChange()
		{
			if(controlGameObjectActivation)
			{
				gameObject.SetActive(Active);
			}
			NotifyChildrenOfActiveChange();
		}
		#endif

		protected override void OnAwake()
		{
			foreach(ActivationControllerBase controller in controllers)
			{
				if(controller == null)
					continue;

				controller.ActivationControllerHub = this;
			}
		}

		void NotifyChildrenOfSetFirstActiveState()
		{
			activatedChildrenCount = 0;
			foreach(ActivationControllerBase controller in controllers)
			{
				if(controller == null)
					continue;
				
				++activatedChildrenCount;
				controller.Activate(Active, true);
			}
		}

		void NotifyChildrenOfActiveChange()
		{
			activatedChildrenCount = 0;
			foreach(ActivationControllerBase controller in controllers)
			{
				if(controller == null)
					continue;
				
				++activatedChildrenCount;
				controller.Active = Active;
			}
		}
	}
}