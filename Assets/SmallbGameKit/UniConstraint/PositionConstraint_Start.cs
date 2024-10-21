using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(10)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniConstraint/PositionConstraint_Start")]
	public class PositionConstraint_Start : PositionConstraint_Base
	{
		void Start()
		{
			UpdateConstraint();
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			Editor_Update();
		}
		#endif
	}
}
