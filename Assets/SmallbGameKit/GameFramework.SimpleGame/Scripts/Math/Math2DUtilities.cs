using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniMath
{
	public class Math2DUtilities
	{
		public static Vector3 Vector2DTo3D(Vector2 point)
		{
			return new Vector3(point.x, 0.0f, point.y);
		}

		public static Vector3 HorizontalPlaneTo3D(Vector2 point, float planeHeight)
		{
			return new Vector3(point.x, planeHeight, point.y);
		}

		public static Vector2 ProjectToHorizontalPlane(Vector3 point)
		{
			return new Vector2(point.x, point.z);
		}

		public static Vector2 ComputePerpendicular(Vector2 vector)
		{
			return new Vector2(vector.y, -vector.x);
		}
	}
}
