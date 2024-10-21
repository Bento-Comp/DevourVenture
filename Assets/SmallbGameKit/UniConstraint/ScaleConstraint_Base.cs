using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[AddComponentMenu("UniConstraint/ScaleConstraint_Base")]
	public abstract class ScaleConstraint_Base : TargetTransformConstraint_Base
	{
		[Header("Constraint")]

		public bool freezeX;
		public bool freezeY;
		public bool freezeZ;

		void OnEnable()
		{
			UpdateConstraint();
		}

		protected override void UpdateConstraint(Transform controlledTransform)
		{
			if(targetTransform == null)
				return;

			Vector3 targetScale = targetTransform.localScale;

			Vector3 wantedScale = controlledTransform.localScale;

			if(freezeX == false)
				wantedScale.x = targetScale.x;

			if(freezeY == false)
				wantedScale.y = targetScale.y;

			if(freezeZ == false)
				wantedScale.z = targetScale.z;

			controlledTransform.localScale = wantedScale;
		}
	}
}
