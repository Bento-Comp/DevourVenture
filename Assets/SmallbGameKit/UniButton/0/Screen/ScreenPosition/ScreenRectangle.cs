using UnityEngine;
using System.Collections;

namespace UniButton
{
	[System.Serializable]
	public class ScreenRectangle
	{
		public Rect rectangle;
		
		public EScreenScalarUnit t;
		
		public EScreenDocking docking;
			
		public Rect GetLocalScreenRectangleInPixel(Camera a_rCamera)
		{
			return ScreenRectangleUtility.GetLocalScreenRectangleInPixel(rectangle, t, a_rCamera);
		}
		
		public Rect GetLocalScreenRectangleInPixel(Rect a_oNormalizeViewport)
		{
			return ScreenRectangleUtility.GetLocalScreenRectangleInPixel(rectangle, t, a_oNormalizeViewport);
		}
		
		public Rect GetScreenRectangleInPixel(Camera a_rCamera)
		{
			return ScreenRectangleUtility.GetScreenRectangleInPixel(rectangle, t, docking, a_rCamera);
		}
		
		public Rect GetScreenRectangleInPixel(Rect a_oNormalizeViewport)
		{
			return ScreenRectangleUtility.GetScreenRectangleInPixel(rectangle, t, docking, a_oNormalizeViewport);
		}
	}
}