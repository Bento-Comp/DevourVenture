using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	public abstract class InputMappingSchemeSelector_Base : MonoBehaviour
	{	
		protected abstract bool ShouldSelect();
		 
		void Awake()
		{
			if(ShouldSelect())
			{
				InputManager.ActivateScheme(gameObject.name);
			}
		}
	}
}