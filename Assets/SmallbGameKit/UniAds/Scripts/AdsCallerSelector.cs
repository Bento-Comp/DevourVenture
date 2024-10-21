using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace UniAds
{
	[DefaultExecutionOrder(-31999)]
	[AddComponentMenu("UniAds/AdsCallerSelector")]
	public abstract class AdsCallerSelector : MonoBehaviour
	{	
		public bool editor_forceSelect;

		protected virtual bool ShouldSelect
		{
			get
			{
				#if UNITY_EDITOR || uniAds_debugSelectAtRuntime
				if(editor_forceSelect)
					return true;
				#endif

				return false;
			}
		}

		void Awake()
		{
			if(ShouldSelect)
			{
				AdsManager.Instance.Select(GetComponent<AdsCaller>());
			}
		}
	}
}
