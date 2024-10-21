using UnityEngine;
using System.Collections;

namespace UniButton
{
	public class GizmoUtility
	{
		static public void DrawScreenRectangleGizmo(Rect a_oScreenRectangle, Color a_oColor, Camera a_rCamera)
		{
			if(a_rCamera == null)
			{
				return;
			}
			
			Vector3 f3TopLeftScreen;
			Vector3 f3TopRightScreen;
			Vector3 f3BottomRightScreen;
			Vector3 f3BottomLeftScreen;
			
			Vector3 f3TopLeft;
			Vector3 f3TopRight;
			Vector3 f3BottomRight;
			Vector3 f3BottomLeft;
			
			float fScreenPlaneDepth;
				
			// Compute the screen plane depth
			fScreenPlaneDepth = 1.0f;
			if(a_rCamera != null)
			{
				fScreenPlaneDepth = a_rCamera.nearClipPlane + (a_rCamera.farClipPlane - a_rCamera.nearClipPlane) * 1e-3f;
			}
				
			// Compute the screen position
			f3BottomLeftScreen = new Vector3(a_oScreenRectangle.xMin, a_oScreenRectangle.yMin, fScreenPlaneDepth);
			f3BottomRightScreen = new Vector3(a_oScreenRectangle.xMax, a_oScreenRectangle.yMin, fScreenPlaneDepth);
			f3TopRightScreen = new Vector3(a_oScreenRectangle.xMax, a_oScreenRectangle.yMax, fScreenPlaneDepth);
			f3TopLeftScreen = new Vector3(a_oScreenRectangle.xMin, a_oScreenRectangle.yMax, fScreenPlaneDepth);	
				
			// Compute the world position
			f3BottomLeft = a_rCamera.ScreenToWorldPoint(f3BottomLeftScreen);
			f3BottomRight = a_rCamera.ScreenToWorldPoint(f3BottomRightScreen);
			f3TopRight = a_rCamera.ScreenToWorldPoint(f3TopRightScreen);
			f3TopLeft = a_rCamera.ScreenToWorldPoint(f3TopLeftScreen);
			
			// Select the gizmo color
			Gizmos.color = a_oColor;
			
			// Draw the gizmo
			Gizmos.DrawLine(f3TopLeft, f3TopRight);
			Gizmos.DrawLine(f3TopRight, f3BottomRight);
			Gizmos.DrawLine(f3BottomRight, f3BottomLeft);
			Gizmos.DrawLine(f3BottomLeft, f3TopLeft);
		}
	}
}