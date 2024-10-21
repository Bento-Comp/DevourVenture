using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/InputMappingSchemeSelector_ByRuntimePlatform")]
	public class InputMappingSchemeSelector_ByRuntimePlatform : InputMappingSchemeSelector_Base
	{	
		public bool invert;

		public List<RuntimePlatform> runtimePlatforms = new List<RuntimePlatform>();
		protected override bool ShouldSelect()
		{
			
			bool shouldSelect = runtimePlatforms.Contains(Application.platform);
			if(invert)
				shouldSelect = !shouldSelect;

			return shouldSelect;
		}
	}
}