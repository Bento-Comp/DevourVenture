using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchZone/TouchZone_RectangleShape")]
	public class TouchZone_RectangleShape : TouchZoneBase
	{
		public RectangleShape rectangleShape;
		
		public bool invertZone;

		public bool Project(Vector2 a_f2ScreenPoint, out Vector3 projectionInWorldSpace)
		{
			return rectangleShape.Project(GetCamera(), a_f2ScreenPoint, out projectionInWorldSpace);
		}


		protected override bool _ContainsScreenPoint(Vector2 a_f2ScreenPoint, Camera a_rCamera)
		{
			bool bInRectangle = rectangleShape.Contains(a_rCamera, a_f2ScreenPoint);
			if(invertZone)
			{
				return !bInRectangle;
			}
			else
			{
				return bInRectangle;
			}
		}
	}
}