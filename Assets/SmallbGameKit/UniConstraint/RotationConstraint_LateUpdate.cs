using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(10)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniConstraint/RotationConstraint_LateUpdate")]
	public class RotationConstraint_LateUpdate : RotationConstraint_Base
	{
		void LateUpdate()
		{
			#if UNITY_EDITOR
			Editor_Update();

			if(Application.isPlaying == false)
				return;
			#endif

			UpdateConstraint();
		}
	}
}
