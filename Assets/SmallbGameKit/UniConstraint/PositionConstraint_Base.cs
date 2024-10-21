using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[AddComponentMenu("UniConstraint/PositionConstraint_Base")]
	public abstract class PositionConstraint_Base : TargetTransformConstraint_Base
	{
		[Header("Constraint")]

		public bool freezeX;
		public bool freezeY;
		public bool freezeZ;

		protected override void UpdateConstraint(Transform controlledTransform)
		{
			if(targetTransform == null)
				return;

			Vector3 targetPosition = targetTransform.position;

			Vector3 wantedPosition = controlledTransform.position;

			if(freezeX == false)
				wantedPosition.x = targetPosition.x;

			if(freezeY == false)
				wantedPosition.y = targetPosition.y;

			if(freezeZ == false)
				wantedPosition.z = targetPosition.z;

			controlledTransform.position = wantedPosition;
		}
	}
}
