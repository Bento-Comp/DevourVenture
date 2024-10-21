using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[DefaultExecutionOrder(10)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniConstraint/LocalPositionController")]
	public class LocalPositionController : TransformConstraint_Base
	{
		protected override void UpdateConstraint(Transform controlledTransform)
		{
			controlledTransform.localPosition = transform.localPosition;
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
