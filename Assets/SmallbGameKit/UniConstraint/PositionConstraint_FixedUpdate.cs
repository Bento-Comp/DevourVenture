using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(10)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniConstraint/PositionConstraint_FixedUpdate")]
	public class PositionConstraint_FixedUpdate : PositionConstraint_Base
	{
		void FixedUpdate()
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
