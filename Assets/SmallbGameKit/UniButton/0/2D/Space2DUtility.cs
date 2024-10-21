using UnityEngine;

namespace UniButton
{
	public static class Space2DUtility
	{
		public static bool ProjectScreenPointOn2DObjectInLocalSpace(Transform a_r2DObjectTransform, Camera a_rCamera, Vector2 a_f2ScreenPoint, out Vector2 a_f2ProjectedPointInLocalSpace)
		{
			Vector3 f3ProjectedPointInLocalSpace;
			bool bSucces = ProjectScreenPointOn2DObjectInLocalSpace(a_r2DObjectTransform, a_rCamera, a_f2ScreenPoint, out f3ProjectedPointInLocalSpace);
			a_f2ProjectedPointInLocalSpace = f3ProjectedPointInLocalSpace;
			
			return bSucces;
		}
	
		public static bool ProjectScreenPointOn2DObjectInLocalSpace(Transform a_r2DObjectTransform, Camera a_rCamera, Vector2 a_f2ScreenPoint, out Vector3 a_f3ProjectedPointInLocalSpace)
		{
			Vector3 f3ProjectedPointInWorldSpace;
			if(ProjectScreenPointOn2DObjectInWorldSpace(a_r2DObjectTransform, a_rCamera, a_f2ScreenPoint, out f3ProjectedPointInWorldSpace))
			{
				a_f3ProjectedPointInLocalSpace = a_r2DObjectTransform.InverseTransformPoint(f3ProjectedPointInWorldSpace);
				return true;
			}
			else
			{
				a_f3ProjectedPointInLocalSpace = Vector3.zero;
				return false;
			}
		}
	
		public static bool ProjectScreenPointOn2DObjectInWorldSpace(Transform a_r2DObjectTransform, Camera a_rCamera, Vector2 a_f2ScreenPoint, out Vector2 a_f2ProjectedPointInWorldSpace)
		{
			Vector3 f3ProjectedPointInWorldSpace;
			bool bSucces = ProjectScreenPointOn2DObjectInWorldSpace(a_r2DObjectTransform, a_rCamera, a_f2ScreenPoint, out f3ProjectedPointInWorldSpace);
			a_f2ProjectedPointInWorldSpace = f3ProjectedPointInWorldSpace;
			
			return bSucces;
		}
		
		public static bool ProjectScreenPointOn2DObjectInWorldSpace(Transform a_r2DObjectTransform, Camera a_rCamera, Vector2 a_f2ScreenPoint, out Vector3 a_f3ProjectedPointInWorldSpace)
		{
			a_f3ProjectedPointInWorldSpace = Vector3.zero;
			
			Plane oRectanglePlane = Compute2DObjectPlane(a_r2DObjectTransform);
			Ray oRay = a_rCamera.ScreenPointToRay(a_f2ScreenPoint);
			float fHitDistanceAlongTheRay;
			if(oRectanglePlane.Raycast(oRay, out fHitDistanceAlongTheRay))
			{
				a_f3ProjectedPointInWorldSpace = oRay.origin + oRay.direction * fHitDistanceAlongTheRay;
				return true;
				
			}
			return false;
		}
		
		public static Plane Compute2DObjectPlane(Transform a_r2DObjectTransform)
		{
			Plane oRectanglePlane;
			oRectanglePlane = new Plane(-a_r2DObjectTransform.forward, a_r2DObjectTransform.position);
			
			return oRectanglePlane;
		}
	}
}
