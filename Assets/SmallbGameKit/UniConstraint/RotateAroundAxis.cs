using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(10)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniConstraint/RotateAroundAxis")]
	public class RotateAroundAxis : TransformConstraint_Base
	{
		public float rotation = 0.0f;

		public Vector3 axis = Vector3.up;

		protected override void UpdateConstraint(Transform controlledTransform)
		{
			controlledTransform.localRotation = Quaternion.AngleAxis(rotation, axis);
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
