using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Juicy;

namespace SmallbGameKit
{
	[DefaultExecutionOrder(-31999)]
	[AddComponentMenu("SmallbGameKit/JuicySDK/ABTest/JuicySDK_ABTest_CohortActivator")]
	public class JuicySDK_ABTest_CohortActivator : MonoBehaviour
	{
		public UniActivation.Activator activator;

		int ActivationIndex
		{
			get
			{
				int cohortIndex = JuicySDK.ABTestCohortVariantIndex;

				if(cohortIndex < 0)
					return 0;

				return cohortIndex;
			}
		}
		void Awake()
		{
			activator.SetFirstActiveState(ActivationIndex);
		}
	}
}
