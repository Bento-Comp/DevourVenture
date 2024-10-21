using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(10)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniConstraint/ScreenPositionConstraint_LateUpdate")]
	public class ScreenPositionConstraint_LateUpdate : ScreenPositionConstraint_Base
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
