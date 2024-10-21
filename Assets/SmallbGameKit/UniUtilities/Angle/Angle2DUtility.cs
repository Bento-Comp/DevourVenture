using UnityEngine;

namespace UniUtilities
{
	public static class Angle2DUtility
	{
		public static Vector2 VectorFromAngle(float angle)
		{
			angle *= Mathf.Deg2Rad;
			Vector2 vector;
			vector.x = Mathf.Cos(angle);
			vector.y = Mathf.Sin(angle);

			return vector;
		}

		public static float Angle(Vector2 vector)
		{
			return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
		}

		public static float DistanceBetweenAngleSigned(Vector2 from, Vector2 to)
		{
			return DistanceBetweenAngleSigned(Angle(from), Angle(to));
		}

		public static float ShortestDistanceBetweenAngleSigned(Vector2 from, Vector2 to)
		{
			return ShortestDistanceBetweenAngleSigned(Angle(from), Angle(to));
		}

		public static float ShortestDistanceBetweenAngleAbs(float angleFrom, float angleTo)
		{
			return Mathf.Abs(ShortestDistanceBetweenAngleSigned(angleFrom, angleTo));
		}

			public static float ShortestDistanceBetweenAngleSigned(float angleFrom, float angleTo)
		{
			float angleFrom360 = ExpressAngleBetween0And360(angleFrom);
			float angleTo360 = ExpressAngleBetween0And360(angleTo);

			float diff360 = angleTo360 - angleFrom360;
			float diff360Abs = Mathf.Abs(diff360);

			float diff360ComplementaryAbs = (360.0f - diff360Abs);
			float diff360Complementary = -Mathf.Sign(diff360) * diff360ComplementaryAbs;

			if(diff360Abs < diff360ComplementaryAbs)
			{
				return diff360;
			}
			else if(diff360ComplementaryAbs < diff360Abs)
			{
				return diff360Complementary;
			}
			else
			{
				return diff360;
			}
		}

		public static float DistanceBetweenAngleSigned(float angleFrom, float angleTo)
		{
			return ExpressAngleBetween180AndMinus180(angleTo) - ExpressAngleBetween180AndMinus180(angleFrom);
		}

		public static float ExpressAngleBetween180AndMinus180(float angle)
		{
			float angleValue = Mathf.Abs(angle);
			angleValue %= 360.0f; 
			if(angleValue > 180.0f)
			{
				angleValue = angleValue - 360.0f;
			}
			
			return angleValue * Mathf.Sign(angle);
		}

		public static float ExpressAngleBetween360AndMinus360(float angle)
		{
			float angleValue = Mathf.Abs(angle);
			angleValue %= 720.0f;
			if(angleValue > 360.0f)
			{
				angleValue = angleValue - 720.0f;
			}

			return angleValue * Mathf.Sign(angle);
		}

		public static float ExpressAngleBetween0AndMinus360(float angle)
		{
			float angleValue = Mathf.Abs(angle);
			angleValue %= 360.0f; 
			if(angleValue > 180.0f)
			{
				angleValue = angleValue - 360.0f;
			}
			if(angle < 0.0f)
			{
				angleValue = 360.0f - angleValue;
			}
			
			return angleValue;
		}

		public static float ExpressAngleBetween0And360(float angle)
		{	
			return ArithmeticUtility.FlooredModulo(angle, 360.0f);
		}

		public static float ProjectAngleTo_Either_0_180_Minus180(float angle)
		{
			float angleBetweenMinus180And180 = ExpressAngleBetween180AndMinus180(angle);

			if(angleBetweenMinus180And180 < -90.0f)
			{
				return -180.0f;
			}
			else if(angleBetweenMinus180And180 > 90.0f)
			{
				return 180.0f;
			}
			else
			{
				return 0.0f;
			}
		}
	}
}
