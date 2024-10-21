using UnityEngine;

namespace UniUtilities
{
	public enum E3DSpaceAxis
	{
		X,
		Y,
		Z
	}
	
	// The 3d space Utility
	public static class _3DSpaceUtility
	{
		public const int mc_i3DSpaceAxisCount = 3;
		
		public static float GetAxisValue(Vector3 a_f3Vector, int a_iAxisIndex)
		{
			return GetAxisValue(a_f3Vector, (E3DSpaceAxis)a_iAxisIndex);
		}
		
		public static float GetAxisValue(Vector3 a_f3Vector, E3DSpaceAxis a_eAxis)
		{
			switch(a_eAxis)
			{
				case E3DSpaceAxis.X:
				{
					return a_f3Vector.x;
				}
				
				case E3DSpaceAxis.Y:
				{
					return a_f3Vector.y;
				}
				
				case E3DSpaceAxis.Z:
				{
					return a_f3Vector.z;
				}
			}
			
			return 0.0f;
		}
		
		public static void SetAxisValue(Vector3 a_f3Vector, int a_iAxisIndex, float a_fValue)
		{
			SetAxisValue(a_f3Vector, (E3DSpaceAxis)a_iAxisIndex, a_fValue);
		}
		
		public static void SetAxisValue(Vector3 a_f3Vector, E3DSpaceAxis a_eAxis, float a_fValue)
		{
			switch(a_eAxis)
			{
				case E3DSpaceAxis.X:
				{
					a_f3Vector.x = a_fValue;
				}
				break;
				
				case E3DSpaceAxis.Y:
				{
					a_f3Vector.y = a_fValue;
				}
				break;
				
				case E3DSpaceAxis.Z:
				{
					a_f3Vector.z = a_fValue;
				}
				break;
			}
		}
	}
}