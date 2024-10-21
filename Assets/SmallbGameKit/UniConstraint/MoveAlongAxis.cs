using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(10)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniConstraint/MoveAlongAxis")]
	public class MoveAlongAxis : TransformConstraint_Base
	{
		public float move = 0.0f;

		public Vector3 axis = Vector3.up;

		protected override void UpdateConstraint(Transform controlledTransform)
		{
			controlledTransform.localPosition = axis * move;
		}

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
