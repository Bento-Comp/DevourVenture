using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniAds
{
	[AddComponentMenu("UniAds/AdsCallerSelector_ByRuntimePlatform")]
	public class AdsCallerSelector_ByRuntimePlatform : AdsCallerSelector
	{	
		public bool invert;

		public List<RuntimePlatform> runtimePlatforms = new List<RuntimePlatform>();

		protected override bool ShouldSelect
		{
			get
			{
				if(base.ShouldSelect)
					return true;
				
				bool shouldSelect = runtimePlatforms.Contains(Application.platform);

				if(invert)
					shouldSelect = !shouldSelect;

				return shouldSelect;
			}
		}
	}
}
