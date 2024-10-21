using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Juicy;

namespace SmallbGameKit
{
	[DefaultExecutionOrder(-31999)]
	[AddComponentMenu("SmallbGameKit/JuicySDK/ABTest/JuicySDK_ABTest_Behaviour")]
	public abstract class JuicySDK_ABTest_Behaviour : MonoBehaviour
	{
		public JuicySDK_ABTest_Cohort cohort;

		protected abstract void OnCohortActive();

		void Awake()
		{
			if(cohort.IsCohortActive == false)
				return;

			OnCohortActive();
		}
	}
}
