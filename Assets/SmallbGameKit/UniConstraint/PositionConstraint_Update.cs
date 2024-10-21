using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(10)]
	[ExecuteAlways]
	[AddComponentMenu("UniConstraint/PositionConstraint_Update")]
	public class PositionConstraint_Update : PositionConstraint_Base
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
