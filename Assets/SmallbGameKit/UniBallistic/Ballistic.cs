using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniBallistic
{
	public static class Ballistic
	{
		// Throw
		static public float ComputeThrowVelocity_By_Distance_Angle_Gravity(float distance, float angle, float gravity)
		{
			float angleRadian = angle * Mathf.Deg2Rad;

			float cosinus = Mathf.Cos(angleRadian);
			float sinus = Mathf.Sin(angleRadian);

			return Mathf.Sqrt( (distance * gravity) / (2.0f * cosinus * sinus) );
		}

		static public float ComputeThrowVelocity_By_Distance_Angle_Gravity_Height(float distance, float angle, float gravity, float height)
		{
			float angleRadian = angle * Mathf.Deg2Rad;

			float cosinus = Mathf.Cos(angleRadian);
			float sinus = Mathf.Sin(angleRadian);

			return Mathf.Sqrt( (distance * gravity) / (2.0f * cosinus * sinus) );
		}

		// Jump
		static public float ComputeJumpDistance_By_VerticalSpeed_HorizontalSpeed_Gravity(float verticalSpeed, float horizontalSpeed, float gravity)
		{
			return 2 * horizontalSpeed * verticalSpeed / gravity;
		}

		static public float ComputeJumpDistance_By_Height_HorizontalSpeed_Gravity(float jumpHeight, float horizontalSpeed, float gravity)
		{
			return 2 * horizontalSpeed * Mathf.Sqrt(2 * jumpHeight / gravity);
		}

		static public float ComputeVerticalJumpVelocity_By_Height_Gravity(float jumpHeight, float gravity)
		{
			return Mathf.Sqrt(2.0f * gravity * jumpHeight);
		}

		static public float ComputeGravity_By_Height_HorizontalSpeed_JumpDistance(float jumpHeight, float horizontalSpeed, float jumpDistance)
		{
			return (8.0f * horizontalSpeed * horizontalSpeed * jumpHeight) / (jumpDistance * jumpDistance);
		}
	}
}
