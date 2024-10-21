using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[AddComponentMenu("UniConstraint/TransformConstraint_Base")]
	public abstract class TargetTransformConstraint_Base : TransformConstraint_Base
	{
		public Transform targetTransform;

		#if UNITY_EDITOR
		protected override void Editor_Update()
		{
			if(targetTransform == null)
				return;

			base.Editor_Update();
		}
		#endif
	}
}
