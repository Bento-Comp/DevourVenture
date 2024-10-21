using UnityEngine;
using System.Collections;

namespace UniButton
{
	public static class ScreenRectangleUtility
	{
		public static Rect GetLocalScreenRectangleInPixel(Rect a_oRectangle, EScreenScalarUnit a_eUnit, Camera a_rCamera)
		{
			return GetLocalScreenRectangleInPixel(a_oRectangle, a_eUnit, a_rCamera.pixelRect);
		}
		
		public static Rect GetLocalScreenRectangleInPixel(Rect a_oRectangle, EScreenScalarUnit a_eUnit, Rect a_oNormalizeViewport)
		{
			Rect oScreenRectangleInPixel = new Rect(0,0,0,0);
			
			// Compute the output rectangle
			oScreenRectangleInPixel.x = ScreenScalarUtility.GetScreenDistanceInPixel(a_oRectangle.x, a_eUnit, EScreenScalarUseOrientation.Width, a_oNormalizeViewport);
			oScreenRectangleInPixel.y = ScreenScalarUtility.GetScreenDistanceInPixel(a_oRectangle.y, a_eUnit, EScreenScalarUseOrientation.Height, a_oNormalizeViewport);
			oScreenRectangleInPixel.width = ScreenScalarUtility.GetScreenDistanceInPixel(a_oRectangle.width, a_eUnit, EScreenScalarUseOrientation.Width, a_oNormalizeViewport);
			oScreenRectangleInPixel.height = ScreenScalarUtility.GetScreenDistanceInPixel(a_oRectangle.height, a_eUnit, EScreenScalarUseOrientation.Height, a_oNormalizeViewport);
			
			return oScreenRectangleInPixel;
		}
		
		public static Rect GetScreenRectangleInPixel(Rect a_oRectangle, EScreenScalarUnit a_eUnit, EScreenDocking a_eDocking, Camera a_rCamera)
		{
			return GetScreenRectangleInPixel(a_oRectangle, a_eUnit, a_eDocking, a_rCamera.pixelRect);
		}
		
		public static Rect GetScreenRectangleInPixel(Rect a_oRectangle, EScreenScalarUnit a_eUnit, EScreenDocking a_eDocking, Rect a_oNormalizeViewport)
		{
			Rect oScreenRectangleInPixel = new Rect(0,0,0,0);
			
			oScreenRectangleInPixel = GetLocalScreenRectangleInPixel(a_oRectangle, a_eUnit, a_oNormalizeViewport);
			oScreenRectangleInPixel = ScreenDockingUtility.ComputeDockedScreenRectangle(oScreenRectangleInPixel, a_eDocking, a_oNormalizeViewport);
			
			return oScreenRectangleInPixel;
		}
	}
}