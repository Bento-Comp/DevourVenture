using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[AddComponentMenu("UniConstraint/ScreenPositionConstraint_Base")]
	public abstract class ScreenPositionConstraint_Base : TargetTransformConstraint_Base
	{
		public Camera constraintCamera;

		protected override void UpdateConstraint(Transform controlledTransform)
		{
			if(targetTransform == null)
				return;

			float controlledDepth = constraintCamera.WorldToViewportPoint(controlledTransform.position).z;

			Vector3 projectedTargetScreen = constraintCamera.WorldToViewportPoint(targetTransform.position);
			projectedTargetScreen.z = controlledDepth;

			Vector3 targetProjectedWorld = constraintCamera.ViewportToWorldPoint(projectedTargetScreen);

			controlledTransform.position = targetProjectedWorld;
		}

#if UNITY_EDITOR
		protected override void Editor_Update()
		{
			if(constraintCamera == null)
				return;

			base.Editor_Update();
		}
#endif
	}
}
