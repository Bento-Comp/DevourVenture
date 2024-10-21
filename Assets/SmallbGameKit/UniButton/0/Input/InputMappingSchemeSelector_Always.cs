using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/InputMappingSchemeSelector_Always")]
	public class InputMappingSchemeSelector_Always : InputMappingSchemeSelector_Base
	{	
		protected override bool ShouldSelect()
		{
			return true;
		}
	}
}