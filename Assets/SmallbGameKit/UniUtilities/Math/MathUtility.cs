using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniUtilities
{
	public class MathUtility
	{
		public static float InverseLerp(float a, float b, float value, bool unclamped)
		{
			if(unclamped)
			{
				return InverseLerpUnclamped(a, b, value);
			}
			else
			{
				return Mathf.InverseLerp(a, b, value);
			}
		}

		public static float Lerp(float a, float b, float t, bool unclamped)
		{
			if(unclamped)
			{
				return Mathf.LerpUnclamped(a, b, t);
			}
			else
			{
				return Mathf.Lerp(a, b, t);
			}
		}

		public static float InverseLerpUnclamped(float a, float b, float value)
		{
			if(b == a)
				return value;

			return (value - a)/(b-a);
		}
	}
}
