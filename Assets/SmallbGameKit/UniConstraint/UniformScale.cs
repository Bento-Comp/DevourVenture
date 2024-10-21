using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[ExecuteInEditMode()]
	[DefaultExecutionOrder(10)]
	[AddComponentMenu("UniConstraint/UniformScale")]
	public class UniformScale : TransformConstraint_Base
	{
		public float scale = 1.0f;

		protected override void UpdateConstraint(Transform controlledTransform)
		{
			controlledTransform.localScale = scale * Vector3.one;
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
