using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{	
	[System.Serializable]
	public class Circle2D
	{	
		public float radius = 1.0f;

		public float innerRadius = 0.0f;

		[Range(0.0f, 180.0f)]
		public float angle = 180.0f;
		
		public bool Contains(Vector2 point)
		{
			float distanceSquared = point.sqrMagnitude;
			if(distanceSquared > radius * radius)
				return false;

			if(distanceSquared < innerRadius * innerRadius)
				return false;

			if(angle >= 180.0f)
				return true;

			float pointAngle = Vector2.Angle(Vector2.up, point);

			return pointAngle <= angle;
		}
	}
}