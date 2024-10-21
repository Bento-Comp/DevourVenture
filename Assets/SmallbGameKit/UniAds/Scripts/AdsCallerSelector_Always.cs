using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniAds
{
	[AddComponentMenu("UniAds/AdsCallerSelector_Always")]
	public class AdsCallerSelector_Always : AdsCallerSelector
	{	
		protected override bool ShouldSelect
		{
			get
			{
				return true;
			}
		}
	}
}
