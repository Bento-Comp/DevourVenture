using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchZone/TouchZone_Collider")]
	public class TouchZone_Collider : TouchZoneBase
	{
		public Collider colliderComponent;
		
		public bool invertZone;

		public bool useTouchColliderRaycastManager;
		public TouchColliderRaycastParameters raycastParameters;

		public bool HasHit => hits.Length > 0;

		public RaycastHit FirstHit => hits.Length > 0 ? hits[0] : new RaycastHit();

		public RaycastHit[] AllHits => hits;

		RaycastHit[] hits;

		protected override bool _ContainsScreenPoint(Vector2 screenPoint, Camera camera)
		{
			Ray ray = camera.ScreenPointToRay(screenPoint);
		
			bool isIn;
			if(useTouchColliderRaycastManager)
			{
				hits = TouchColliderManager.Instance.Raycast(ray, raycastParameters);
				
				isIn = HitsContainCollider(hits);
			}
			else
			{
				RaycastHit hit;
				isIn = colliderComponent.Raycast(ray, out hit, float.PositiveInfinity);
				hits = new RaycastHit[] { hit };
			}

			if(invertZone)
			{
				return !isIn;
			}
			else
			{
				return isIn;
			}
		}

		bool HitsContainCollider(RaycastHit[] hits)
		{
			for(int i = 0; i < hits.Length; ++i)
			{
				if(hits[i].collider == colliderComponent)
					return true;
			}

			return false;
		}
	}
}