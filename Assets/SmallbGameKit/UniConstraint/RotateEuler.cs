using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(10)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniConstraint/RotateEuler")]
	public class RotateEuler : TransformConstraint_Base
	{
		public float x;
		public float y;
		public float z;

		protected override void UpdateConstraint(Transform controlledTransform)
		{
			controlledTransform.localEulerAngles = new Vector3(x, y, z);
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
