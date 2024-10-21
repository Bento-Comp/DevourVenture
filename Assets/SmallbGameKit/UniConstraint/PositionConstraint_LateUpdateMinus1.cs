using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(-1)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniConstraint/PositionConstraint_LateUpdateMinus1")]
	public class PositionConstraint_LateUpdateMinus1 : PositionConstraint_Base
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
