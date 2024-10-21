using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniUtilities
{
	public class PhysicsUtility
	{
		public static Vector3 ApplyDrag(Vector3 velocity, float drag, float deltaTime,
			float considerVelocityAsNullEpsilon = 0.01f)
		{
			float multiplier = 1.0f - drag * deltaTime;

			if (multiplier < 0.0f)
			{
				multiplier = 0.0f;
			}

			velocity *= multiplier;

			if(velocity.magnitude <= considerVelocityAsNullEpsilon)
				return Vector3.zero;

			return velocity;
		}

		public static Vector2 ApplyDrag(Vector2 velocity, float drag, float deltaTime,
			float considerVelocityAsNullEpsilon = 0.01f)
		{
			float multiplier = 1.0f - drag * deltaTime;

			if (multiplier < 0.0f)
			{
				multiplier = 0.0f;
			}

			velocity *= multiplier;

			if(velocity.magnitude <= considerVelocityAsNullEpsilon)
				return Vector2.zero;

			return velocity;
		}

		public static float ApplyDrag(float velocity, float drag, float deltaTime,
			float considerVelocityAsNullEpsilon = 0.01f)
		{
			float multiplier = 1.0f - drag * deltaTime;

			if (multiplier < 0.0f)
			{
				multiplier = 0.0f;
			}

			velocity *= multiplier;

			if(Mathf.Abs(velocity) <= considerVelocityAsNullEpsilon)
				return 0.0f;

			return velocity;
		}

		public static Vector3 GotoTarget_Gravity_AlwaysToward(Vector3 current, Vector3 target, ref float velocity,
			float gravity = 9.81f,
			float reachTargetRadius = 0.0f)
		{
			bool targetReached;
			return GotoTarget_Gravity_AlwaysToward(current, target, ref velocity, out targetReached, gravity, reachTargetRadius);
		}

		public static Vector3 GotoTarget_Gravity_AlwaysToward(Vector3 current, Vector3 target, ref float velocity,
			out bool targetReached,
			float gravity = 9.81f,
			float reachTargetRadius = 0.0f)
		{
			velocity += Time.deltaTime * gravity;

			float movementLength = velocity * Time.deltaTime;

			if(IsMovementCrossingTarget(current, movementLength, target, reachTargetRadius))
			{
				velocity = 0.0f;
				targetReached = true;
				return target;
			}

			Vector3 direction = (target - current).normalized;

			targetReached = false;
			return current + direction * movementLength;
		}

		public static float GotoTarget_Gravity_AlwaysToward(float current, float target, ref float velocity,
			float gravity = 9.81f,
			float reachTargetRadius = 0.0f)
		{
			bool targetReached;
			return GotoTarget_Gravity_AlwaysToward(current, target, ref velocity, out targetReached, gravity, reachTargetRadius);
		}

		public static float GotoTarget_Gravity_AlwaysToward(float current, float target, ref float velocity,
			out bool targetReached,
			float gravity = 9.81f,
			float reachTargetRadius = 0.0f)
		{
			velocity += Time.deltaTime * gravity;

			float movementLength = velocity * Time.deltaTime;

			if(IsMovementCrossingTarget(current, movementLength, target, reachTargetRadius))
			{
				velocity = 0.0f;
				targetReached = true;
				return target;
			}

			float direction = Mathf.Sign(target - current);

			targetReached = false;
			return current + direction * movementLength;
		}

		public static bool IsMovementCrossingTarget(Vector3 start, float movementLength, Vector3 target, float reachTargetRadius)
		{
			float distanceToTarget = Vector3.Distance(start, target);
			return movementLength >= distanceToTarget - reachTargetRadius;
		}

		public static bool IsMovementCrossingTarget(float start, float movementLength, float target, float reachTargetRadius)
		{
			float distanceToTarget = Mathf.Abs(target - start);
			return movementLength >= distanceToTarget - reachTargetRadius;
		}
	}
}
