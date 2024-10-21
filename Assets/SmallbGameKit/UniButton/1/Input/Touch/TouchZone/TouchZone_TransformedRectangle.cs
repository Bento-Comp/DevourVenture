using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchZone/TouchZone_TransformedRectangle")]
	public class TouchZone_TransformedRectangle : TouchZoneBase
	{
		public Rectangle rectangle;
		
		public Color gizmoColor = Color.red;
		
		protected override bool _ContainsScreenPoint(Vector2 a_f2ScreenPoint, Camera a_rCamera)
		{
			return rectangle.Contains(transform, a_rCamera, a_f2ScreenPoint);
		}
		
		protected override void DisplayGizmos(Camera a_rCamera)
		{
			if(rectangle != null)
			{
				rectangle.DisplayLinesGizmos(transform, gizmoColor);
			}
		}
	}
}