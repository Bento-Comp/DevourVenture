using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[AddComponentMenu("UniConstraint/ConstraintUtility")]
	public class ConstraintUtility
	{
		// Frame Rate Independant Damp
		// http://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/
		
		public static float Damp(float current, float target, float lambda, float dt)
		{
			return Mathf.Lerp(target, current, 1.0f - Mathf.Exp(-lambda * dt));
		}

		public static float DampAngle(float current, float target, float lambda, float dt)
		{
			return Mathf.LerpAngle(target, current, 1.0f - Mathf.Exp(-lambda * dt));
		}

		public static float SimpleDampAngle(float current, float target, float lambda, float dt)
		{
			return Mathf.LerpAngle(current, target, lambda * dt);
		}

		public static Vector3 Damp(Vector3 current, Vector3 target, float lambda, float dt)
		{
			return Vector3.Lerp(target, current, 1.0f - Mathf.Exp(-lambda * dt));
		}

		public static Vector2 Damp(Vector2 current, Vector2 target, float lambda, float dt)
		{
			return Vector2.Lerp(target, current, 1.0f - Mathf.Exp(-lambda * dt));
		}

		public static Quaternion Damp(Quaternion current, Quaternion target, float lambda, float dt)
		{
			return Quaternion.Lerp(target, current, 1.0f - Mathf.Exp(-lambda * dt));
		}

		public static Quaternion SDamp(Quaternion current, Quaternion target, float lambda, float dt)
		{
			return Quaternion.Slerp(target, current, 1.0f - Mathf.Exp(-lambda * dt));
		}

		public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
		{
			if (Time.deltaTime < Mathf.Epsilon) return rot;
			// account for double-cover
			var Dot = Quaternion.Dot(rot, target);
			var Multi = Dot > 0.0f ? 1.0f : -1.0f;
			target.x *= Multi;
			target.y *= Multi;
			target.z *= Multi;
			target.w *= Multi;
			// smooth damp (nlerp approx)
			var Result = new Vector4(
				Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
				Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
				Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
				Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
			).normalized;
		
			// ensure deriv is tangent
			var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
			deriv.x -= derivError.x;
			deriv.y -= derivError.y;
			deriv.z -= derivError.z;
			deriv.w -= derivError.w;		
		
			return new Quaternion(Result.x, Result.y, Result.z, Result.w);
		}
	}
}
