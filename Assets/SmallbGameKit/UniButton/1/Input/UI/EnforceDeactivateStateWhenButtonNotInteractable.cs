using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/EnforceDeactivateStateWhenButtonNotInteractable")]
	public class EnforceDeactivateStateWhenButtonNotInteractable : MonoBehaviour
	{
		public UnityEngine.UI.Button uiButton;

		void LateUpdate()
		{
			UpdateState();
		}

		void UpdateState()
		{
			////Debug.Log("EnforceDeactivateStateWhenButtonNotInteractable : " + uiButton.IsInteractable());
			if(uiButton.IsInteractable())
			{
				if(uiButton.animator.GetCurrentAnimatorStateInfo(0).IsName(uiButton.animationTriggers.normalTrigger) == false)
				{
					uiButton.animator.Update(0.0f);
				}
			}
			else
			{
				if(uiButton.animator.GetCurrentAnimatorStateInfo(0).IsName(uiButton.animationTriggers.disabledTrigger) == false)
				{
					uiButton.animator.SetTrigger(uiButton.animationTriggers.disabledTrigger);
					uiButton.animator.Update(0.0f);
				}
			}
		}
	}
}