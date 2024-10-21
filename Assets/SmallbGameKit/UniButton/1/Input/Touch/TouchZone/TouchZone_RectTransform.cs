using UnityEngine;
using System.Collections;
using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchZone/TouchZone_RectTransform")]
	public class TouchZone_RectTransform : TouchZoneBase
	{
		public RectTransform rectTransform;
		
		public bool invertZone;

		protected override bool _ContainsScreenPoint(Vector2 screenPoint, Camera camera)
		{
			bool inRect = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPoint, camera);
			if(invertZone)
			{
				return !inRect;
			}
			else
			{
				return inRect;
			}
		}
	}
}