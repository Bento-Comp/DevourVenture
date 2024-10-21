using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(10)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniConstraint/RotationConstraint_FixedUpdate")]
	public class RotationConstraint_FixedUpdate : RotationConstraint_Base
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
