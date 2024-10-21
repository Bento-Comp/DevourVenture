using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchZone/TouchZone_Circle")]
	public class TouchZone_Circle : TouchZoneBase
	{
		public Circle2DWithTransform circle;
		
		public Color gizmoColor = Color.red;
		
		protected override bool _ContainsScreenPoint(Vector2 screenPoint, Camera camera)
		{
			return circle.Contains(screenPoint, camera);
		}
		
		protected override void DisplayGizmos(Camera camera)
		{
			if(circle != null)
			{
				circle.DisplayGizmos(gizmoColor);
			}
		}
	}
}