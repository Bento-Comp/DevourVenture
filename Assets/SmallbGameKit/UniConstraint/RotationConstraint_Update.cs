using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(10)]
	[ExecuteAlways]
	[AddComponentMenu("UniConstraint/RotationConstraint_Update")]
	public class RotationConstraint_Update : RotationConstraint_Base
	{
		void Update()
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
