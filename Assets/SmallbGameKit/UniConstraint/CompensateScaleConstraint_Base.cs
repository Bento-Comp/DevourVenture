using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[AddComponentMenu("UniConstraint/CompensateScaleConstraint_Base")]
	public abstract class CompensateScaleConstraint_Base : TargetTransformConstraint_Base
	{
		[Header("Constraint")]
		public bool freezeX;
		public bool freezeY;
		public bool freezeZ;

		protected override void UpdateConstraint(Transform controlledTransform)
		{
			Vector3 targetScale = controlledTransform.localScale;

			Vector3 wantedScale = controlledTransform.localScale;

			if(freezeX == false)
				wantedScale.x = CompensateScale(targetScale.x);

			if(freezeY == false)
				wantedScale.y = CompensateScale(targetScale.y);

			if(freezeZ == false)
				wantedScale.z = CompensateScale(targetScale.z);

			controlledTransform.localScale = wantedScale;
		}

		float CompensateScale(float scaleToCompensate)
		{
			if(scaleToCompensate == 0.0f)
				return 1.0f;

			return 1.0f/scaleToCompensate;
		}
	}
}
