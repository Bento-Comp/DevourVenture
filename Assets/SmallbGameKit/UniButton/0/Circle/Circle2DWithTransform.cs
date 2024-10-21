using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[System.Serializable]
	public class Circle2DWithTransform
	{
		public Transform polygonXYPlaneTransform;
		
		public Circle2D circle2D;

		public bool Contains(Vector2 screenPoint, Camera camera = null)
		{
			if(circle2D == null)
			{
				return false;
			}
			
			Vector2 point2D = ScreenPointToLocalSpace(screenPoint, camera);
			return circle2D.Contains(point2D);
		}

		public void DisplayGizmos(Color color, int displaySegmentCount = 36)
		{
			Color colorSave = Gizmos.color;
			Gizmos.color = color;

			float coneAngleRadians = Mathf.Deg2Rad * circle2D.angle;
			float radius = circle2D.radius;
			float innerRadius = circle2D.innerRadius;

			// Perimeter line
			DrawCircleLine(radius, coneAngleRadians, displaySegmentCount);
			if(innerRadius > 0.0f)
			{
				DrawCircleLine(innerRadius, coneAngleRadians, displaySegmentCount);
			}
			// Side lines
			if(coneAngleRadians < Mathf.PI)
			{
				Vector3 left = AngleToTransformedCircleVertex(-coneAngleRadians, radius);
				Vector3 right = AngleToTransformedCircleVertex(coneAngleRadians, radius);

				Vector3 innerLeft = AngleToTransformedCircleVertex(-coneAngleRadians, innerRadius);
				Vector3 innerRight = AngleToTransformedCircleVertex(coneAngleRadians, innerRadius);

				Gizmos.DrawLine(innerLeft, left);

				Gizmos.DrawLine(innerRight, right);
			}

			Gizmos.color = colorSave;
		}

		void DrawCircleLine(float radius, float coneAngleRadians, int displaySegmentCount)
		{
			float stepAngle = (2.0f * coneAngleRadians)/displaySegmentCount;
			float currentAngle = -coneAngleRadians;
			float nextAngle = currentAngle + stepAngle;
			for(int i = 0; i < displaySegmentCount; ++i)
			{
				Vector3 currentPoint = AngleToTransformedCircleVertex(currentAngle, radius);
				Vector3 nextPoint = AngleToTransformedCircleVertex(nextAngle, radius);

				Gizmos.DrawLine(currentPoint, nextPoint);

				currentAngle += stepAngle;
				nextAngle += stepAngle;
			}
		}

		Vector3 AngleToTransformedCircleVertex(float angleRadians, float radius)
		{
			return TransformLocal2DPointIntoWorldSpace(AngleToCircleVertex(angleRadians, radius));
		}

		Vector2 AngleToCircleVertex(float angleRadians, float radius)
		{
			// zero angle is up
			return new Vector2(Mathf.Sin(angleRadians), Mathf.Cos(angleRadians)) * radius; 
		}
		
		Vector2 ScreenPointToLocalSpace(Vector2 screenPoint, Camera camera)
		{
			Ray ray = camera.ScreenPointToRay(screenPoint);

			Vector3 projectedPoint;
			RaycastPolygonPlane(ray, out projectedPoint);

			Vector3 projectedPointInLocalSpace = polygonXYPlaneTransform.InverseTransformPoint(projectedPoint);
			
			return new Vector2(projectedPointInLocalSpace.x, projectedPointInLocalSpace.y);
		}

		bool RaycastPolygonPlane(Ray a_oRay, out Vector3 hitPoint)
		{
			float hitDistanceAlongTheRay;
			return RaycastPolygonPlane(a_oRay, out hitPoint, out hitDistanceAlongTheRay);
		}

		bool RaycastPolygonPlane(Ray ray, out Vector3 hitPoint, out float hitDistanceAlongTheRay)
		{
			hitPoint = Vector3.zero;
			
			Plane polygonePlane;
			if(polygonXYPlaneTransform == null)
			{
				polygonePlane = new Plane(Vector3.back, Vector3.zero);
			}
			else
			{
				polygonePlane = new Plane(-polygonXYPlaneTransform.forward, polygonXYPlaneTransform.position);
			}
			if(polygonePlane.Raycast(ray, out hitDistanceAlongTheRay))
			{
				hitPoint = ray.origin + ray.direction * hitDistanceAlongTheRay;
				return true;
			}
			return false;
		}

		Vector3 TransformLocal2DPointIntoWorldSpace(Vector2 pointInLocalSpace)
		{
			Vector3 pointInLocalSpace3d = new Vector3(pointInLocalSpace.x, pointInLocalSpace.y, 0.0f);
			
			if(polygonXYPlaneTransform == null)
			{
				return pointInLocalSpace3d;	
			}
			
			Vector3 pointInWorldSpace = polygonXYPlaneTransform.TransformPoint(pointInLocalSpace3d);
			
			return pointInWorldSpace;
		}
	}
}
