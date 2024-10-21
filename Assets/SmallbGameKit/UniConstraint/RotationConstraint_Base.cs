using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[AddComponentMenu("UniConstraint/RotationConstraint_Base")]
	public abstract class RotationConstraint_Base : TargetTransformConstraint_Base
	{
		[Header("Constraint")]

		public bool local;

		public float damping = 0.0f;

		public bool freezeX;
		public bool freezeY;
		public bool freezeZ;

		protected override void UpdateConstraint(Transform controlledTransform)
		{
			if(targetTransform == null)
				return;

			if(damping <= 0.0f || Application.isPlaying == false)
			{
				if(local)
				{
					if(freezeX || freezeY || freezeZ)
					{
						Vector3 wantedRotation = controlledTransform.localEulerAngles;
						Vector3 targetRotation = targetTransform.localEulerAngles;

						if(freezeX == false)
							wantedRotation.x = targetRotation.x;

						if(freezeY == false)
							wantedRotation.y = targetRotation.y;

						if(freezeZ == false)
							wantedRotation.z = targetRotation.z;

						controlledTransform.localEulerAngles = wantedRotation;
					}
					else
					{
						controlledTransform.localRotation = targetTransform.localRotation;
					}
				}
				else
				{
					if(freezeX || freezeY || freezeZ)
					{
						Vector3 wantedRotation = controlledTransform.eulerAngles;
						Vector3 targetRotation = targetTransform.eulerAngles;

						if(freezeX == false)
							wantedRotation.x = targetRotation.x;

						if(freezeY == false)
							wantedRotation.y = targetRotation.y;

						if(freezeZ == false)
							wantedRotation.z = targetRotation.z;

						controlledTransform.eulerAngles = wantedRotation;
					}
					else
					{
						controlledTransform.rotation = targetTransform.rotation;
					}
				}
			}
			else
			{

				if(freezeX || freezeY || freezeZ)
				{
					Vector3 currentRotation = local ?
						controlledTransform.localEulerAngles
						: controlledTransform.eulerAngles;

					Vector3 targetRotation = local ?
						targetTransform.localEulerAngles
						: targetTransform.eulerAngles;

					Vector3 appliedRotation = currentRotation;

					if(freezeX == false)
							appliedRotation.x = ConstraintUtility.DampAngle(
								currentRotation.x, targetRotation.x, damping, Time.deltaTime);

					if(freezeY == false)
							appliedRotation.y = ConstraintUtility.SimpleDampAngle(
								currentRotation.y, targetRotation.y, damping, Time.deltaTime);

					if(freezeZ == false)
							appliedRotation.z = ConstraintUtility.DampAngle(
								currentRotation.z, targetRotation.z, damping, Time.deltaTime);

					if(local)
					{
						controlledTransform.localEulerAngles = appliedRotation;
					}
					else
					{
						controlledTransform.eulerAngles = appliedRotation;
					}
				}
				else
				{
					controlledTransform.rotation =
						ConstraintUtility.SDamp(controlledTransform.rotation, targetTransform.rotation, damping,
						Time.deltaTime);
				}
			}
		}
	}
}
