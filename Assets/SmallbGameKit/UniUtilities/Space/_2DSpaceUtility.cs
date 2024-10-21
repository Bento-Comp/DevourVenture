using UnityEngine;

namespace UniUtilities
{
	public enum E2DSpaceAxis
	{
		X,
		Y
	}
	
	public static class _2DSpaceUtility
	{
		public const int mc_i2DSpaceAxisCount = 2;
		
		public static int GetComplementaryAxisTypeIndex(int a_iAxisIndex)
		{
			return (a_iAxisIndex + 1) % mc_i2DSpaceAxisCount;
		}
		
		public static E2DSpaceAxis GetComplementaryAxisType(E2DSpaceAxis a_eAxis)
		{
			return (E2DSpaceAxis)GetComplementaryAxisTypeIndex((int)a_eAxis);
		}
		
		public static float GetComplementaryAxisValue(Vector2 a_f2Vector, int a_iAxisIndex)
		{
			return GetAxisValue(a_f2Vector, GetComplementaryAxisTypeIndex(a_iAxisIndex));
		}
		
		public static float GetComplementaryAxisValue(Vector2 a_f2Vector, E2DSpaceAxis a_eAxis)
		{
			return GetAxisValue(a_f2Vector, GetComplementaryAxisType(a_eAxis));
		}
		
		public static void SetComplementaryAxisValue(Vector2 a_f2Vector, int a_iAxisIndex, float a_fValue)
		{
			SetAxisValue(a_f2Vector, GetComplementaryAxisTypeIndex(a_iAxisIndex), a_fValue);
		}
		
		public static void SetComplementaryAxisValue(Vector2 a_f2Vector, E2DSpaceAxis a_eAxis, float a_fValue)
		{
			SetAxisValue(a_f2Vector, GetComplementaryAxisType(a_eAxis), a_fValue);
		}
		
		public static Vector2 GetComplementaryAxis(int a_iAxisIndex)
		{
			return GetAxis(GetComplementaryAxisTypeIndex(a_iAxisIndex));
		}
		 
		public static Vector2 GetComplementaryAxis(E2DSpaceAxis a_eAxis)
		{
			return GetAxis(GetComplementaryAxisType(a_eAxis));
		}
		
		public static Vector2 GetComplementaryAxis(Vector2 a_f2Vector, int a_iAxisIndex)
		{
			return GetAxis(a_f2Vector, GetComplementaryAxisTypeIndex(a_iAxisIndex));
		}
		 
		public static Vector2 GetComplementaryAxis(Vector2 a_f2Vector, E2DSpaceAxis a_eAxis)
		{
			return GetAxis(a_f2Vector,GetComplementaryAxisType(a_eAxis));
		}
		
		public static Vector2 GetAxis(int a_iAxisIndex)
		{
			return GetAxis((E2DSpaceAxis)a_iAxisIndex);
		}
		
		public static Vector2 GetAxis(E2DSpaceAxis a_eAxis)
		{
			switch(a_eAxis)
			{
				case E2DSpaceAxis.X:
				{
					return Vector2.right;
				}
				
				case E2DSpaceAxis.Y:
				{
					return Vector2.up;
				}
			}
			
			return Vector2.zero;
		}
		
		public static Vector2 GetAxis(Vector2 a_f2Vector, int a_iAxisIndex)
		{
			return GetAxis(a_f2Vector, (E2DSpaceAxis)a_iAxisIndex);
		}
		
		public static Vector2 GetAxis(Vector2 a_f2Vector, E2DSpaceAxis a_eAxis)
		{
			switch(a_eAxis)
			{
				case E2DSpaceAxis.X:
				{
					return Vector2.right * a_f2Vector.x;
				}
				
				case E2DSpaceAxis.Y:
				{
					return Vector2.up * a_f2Vector.y;
				}
			}
			
			return Vector2.zero;
		}
		
		public static float GetAxisValue(Vector2 a_f2Vector, int a_iAxisIndex)
		{
			return GetAxisValue(a_f2Vector, (E2DSpaceAxis)a_iAxisIndex);
		}
		
		public static float GetAxisValue(Vector2 a_f2Vector, E2DSpaceAxis a_eAxis)
		{
			switch(a_eAxis)
			{
				case E2DSpaceAxis.X:
				{
					return a_f2Vector.x;
				}
				
				case E2DSpaceAxis.Y:
				{
					return a_f2Vector.y;
				}
			}
			
			return 0.0f;
		}
		
		public static void SetAxisValue(Vector2 a_f2Vector, int a_iAxisIndex, float a_fValue)
		{
			SetAxisValue(a_f2Vector, (E2DSpaceAxis)a_iAxisIndex, a_fValue);
		}
		
		public static void SetAxisValue(Vector2 a_f2Vector, E2DSpaceAxis a_eAxis, float a_fValue)
		{
			switch(a_eAxis)
			{
				case E2DSpaceAxis.X:
				{
					a_f2Vector.x = a_fValue;
				}
				break;
				
				case E2DSpaceAxis.Y:
				{
					a_f2Vector.y = a_fValue;
				}
				break;
			}
		}
	}
}
