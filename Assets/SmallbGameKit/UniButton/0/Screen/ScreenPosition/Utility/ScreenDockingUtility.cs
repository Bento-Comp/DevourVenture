using UnityEngine;
using System.Collections;

namespace UniButton
{
	public enum EScreenDocking
	{
		BottomLeft,
		BottomRight,
		TopLeft,
		TopRight,
		Center
	}
	
	public static class ScreenDockingUtility
	{
		public static bool LeftDocking(EScreenDocking a_eScreenDocking)
		{
			return a_eScreenDocking == EScreenDocking.BottomLeft || a_eScreenDocking == EScreenDocking.TopLeft;
		}
		
		public static bool RightDocking(EScreenDocking a_eScreenDocking)
		{
			return a_eScreenDocking == EScreenDocking.BottomRight || a_eScreenDocking == EScreenDocking.TopRight;
		}
		
		public static bool BottomDocking(EScreenDocking a_eScreenDocking)
		{
			return a_eScreenDocking == EScreenDocking.BottomLeft || a_eScreenDocking == EScreenDocking.BottomRight;
		}
		
		public static bool TopDocking(EScreenDocking a_eScreenDocking)
		{
			return a_eScreenDocking == EScreenDocking.TopLeft || a_eScreenDocking == EScreenDocking.TopRight;
		}
		
		public static bool CenterDocking(EScreenDocking a_eScreenDocking)
		{
			return a_eScreenDocking == EScreenDocking.Center;
		}
		
		public static float ComputeDockedScreenPoint(float a_fScalarOffset, EScreenScalarUseOrientation a_eOrientation, EScreenDocking a_eDocking, Rect a_oNormalizedViewportRectangle)
		{
			float fScreenPointCoordinate = 0.0f;
			switch(a_eOrientation)
			{	
				case EScreenScalarUseOrientation.Height:
				{
					if(BottomDocking(a_eDocking))
					{
						fScreenPointCoordinate =  a_oNormalizedViewportRectangle.yMin + a_fScalarOffset;
					}
					else if(TopDocking(a_eDocking))
					{
						fScreenPointCoordinate = a_oNormalizedViewportRectangle.yMax - a_fScalarOffset;
					}
					else if(CenterDocking(a_eDocking))
					{
						fScreenPointCoordinate = a_oNormalizedViewportRectangle.center.y + a_fScalarOffset;
					}
				}
				break;
				
				case EScreenScalarUseOrientation.Width:
				{
					if(LeftDocking(a_eDocking))
					{
						fScreenPointCoordinate =  a_oNormalizedViewportRectangle.xMin + a_fScalarOffset;
					}
					else if(RightDocking(a_eDocking))
					{
						fScreenPointCoordinate =  a_oNormalizedViewportRectangle.xMax - a_fScalarOffset;
					}
					else if(CenterDocking(a_eDocking))
					{
						fScreenPointCoordinate = a_oNormalizedViewportRectangle.center.x + a_fScalarOffset;
					}
				}
				break;
			}
			
			return fScreenPointCoordinate;
		}
		
		public static Rect ComputeDockedScreenRectangle(Rect a_oLocalRectangle, EScreenDocking a_eDocking, Rect a_oNormalizedViewportRectangle)
		{
			Vector2 f2RectangleBottomLeft = Vector2.zero;
			
			if(LeftDocking(a_eDocking))
			{
				f2RectangleBottomLeft.x =  a_oNormalizedViewportRectangle.xMin + a_oLocalRectangle.xMin;
			}
			else if(RightDocking(a_eDocking))
			{
				f2RectangleBottomLeft.x =  a_oNormalizedViewportRectangle.xMax - a_oLocalRectangle.xMax;
			}
			else if(CenterDocking(a_eDocking))
			{
				f2RectangleBottomLeft.x = a_oNormalizedViewportRectangle.center.x - a_oLocalRectangle.center.x;
			}
			
			if(BottomDocking(a_eDocking))
			{
				f2RectangleBottomLeft.y =  a_oNormalizedViewportRectangle.yMin + a_oLocalRectangle.yMin;
			}
			else if(TopDocking(a_eDocking))
			{
				f2RectangleBottomLeft.y = a_oNormalizedViewportRectangle.yMax - a_oLocalRectangle.yMax;
			}
			else if(CenterDocking(a_eDocking))
			{
				f2RectangleBottomLeft.y = a_oNormalizedViewportRectangle.center.y - a_oLocalRectangle.center.y;
			}
			
			a_oLocalRectangle.x = f2RectangleBottomLeft.x;
			a_oLocalRectangle.y = f2RectangleBottomLeft.y;
			
			return a_oLocalRectangle;
		}
	}
}