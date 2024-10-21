using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchZone/TouchZone_Collider2D")]
	public class TouchZone_Collider2D : TouchZoneBase
	{
		public Collider2D collider2DComponent;
		
		public bool invertZone;
		
		protected override bool _ContainsScreenPoint(Vector2 a_f2ScreenPoint, Camera a_rCamera)
		{
			Vector2 f2TestPoint;
			if(Space2DUtility.ProjectScreenPointOn2DObjectInWorldSpace(collider2DComponent.transform, a_rCamera, a_f2ScreenPoint, out f2TestPoint))
			{
				bool bIn = collider2DComponent.OverlapPoint(f2TestPoint);
				if(invertZone)
				{
					return !bIn;
				}
				else
				{
					return bIn;
				}
			}
			else
			{
				return false;
			}
		}
	}
}