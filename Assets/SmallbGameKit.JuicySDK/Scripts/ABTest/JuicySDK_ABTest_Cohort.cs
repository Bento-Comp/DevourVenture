using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Juicy;

namespace SmallbGameKit
{
	[AddComponentMenu("SmallbGameKit/JuicySDK/ABTest/JuicySDK_ABTest_Cohort")]
	public class JuicySDK_ABTest_Cohort : MonoBehaviour
	{
		public int cohortIndex = 1;

		public bool IsCohortActive => cohortIndex == JuicySDK.ABTestCohortVariantIndex;
	}
}
