using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Polygon/PolygonShape")]
	public class PolygonShape : MonoBehaviour
	{
		public Polygon2DWithTransform polygon = new Polygon2DWithTransform(EPolygonPrimitive.Square);
		
		public Color gizmoColor = Color.red;
		
		void OnDrawGizmos()
		{
			if(polygon != null)
			{
				polygon.DisplayLinesGizmos(gizmoColor);
			}
		}
	}
}