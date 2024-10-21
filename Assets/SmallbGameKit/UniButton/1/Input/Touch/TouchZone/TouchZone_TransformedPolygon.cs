using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchZone/TouchZone_TransformedPolygon")]
	public class TouchZone_TransformedPolygon : TouchZoneBase
	{
		public Polygon2DWithTransform polygon = new Polygon2DWithTransform(EPolygonPrimitive.Square);
		
		public Color gizmoColor = Color.red;
		
		protected override bool _ContainsScreenPoint(Vector2 screenPoint, Camera camera)
		{
			return polygon.ContainsAsConvex(screenPoint, GetComponent<Camera>());
		}
		
		protected override void DisplayGizmos(Camera camera)
		{
			if(polygon != null)
			{
				polygon.DisplayLinesGizmos(gizmoColor);
			}
		}
	}
}