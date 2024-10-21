using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[System.Serializable]
	public class Rectangle
	{
		public float x;
		
		public float y;
		
		public float width = 1.0f;
		
		public float height = 1.0f;
		
		public int VertexCount
		{
			get
			{
				return 4;
			}
		}
		
		public Vector2 Size
		{
			get
			{
				return new Vector2(width, height);
			}
	
			set
			{
				width = value.x;
				height = value.y;
			}
		}
		
		public Vector2 Extent
		{
			get
			{
				return new Vector2(width, height) * 0.5f;
			}
		}
		
		public Vector2 Center
		{
			get
			{
				return new Vector2(x, y);
			}
		}
		
		public Vector2 Left
		{
			get
			{
				return Center + new Vector2(-width, 0.0f) * 0.5f;
			}
		}
		
		public Vector2 Right
		{
			get
			{
				return Center + new Vector2(width, 0.0f) * 0.5f;
			}
		}
		
		public Vector2 Top
		{
			get
			{
				return Center + new Vector2(0.0f, height) * 0.5f;
			}
		}
		
		public Vector2 Bottom
		{
			get
			{
				return Center + new Vector2(0.0f, -height) * 0.5f;
			}
		}
		
		public Vector2 TopLeft
		{
			get
			{
				return Center + new Vector2(-width, height) * 0.5f;
			}
		}
		
		public Vector2 TopRight
		{
			get
			{
				return Center + new Vector2(width, height) * 0.5f;
			}
		}
		
		public Vector2 BottomRight
		{
			get
			{
				return Center + new Vector2(width, -height) * 0.5f;
			}
		}
		
		public Vector2 BottomLeft
		{
			get
			{
				return Center + new Vector2(-width, -height) * 0.5f;
			}
		}
		
		public List<Vector3> GetLocalVertices()
		{	
			List<Vector3> oVertices = new List<Vector3>();
			
			Rect oRect = GetRect();
			
			Vector2 f2VertexBottomLeft = new Vector2(oRect.xMin, oRect.yMin);
			Vector2 f2VertexTopLeft = new Vector2(oRect.xMin, oRect.yMax);
			Vector2 f2VertexTopRight = new Vector2(oRect.xMax, oRect.yMax);
			Vector2 f2VertexBottomRight = new Vector2(oRect.xMax, oRect.yMin);
			
			oVertices.Add(f2VertexBottomLeft);
			oVertices.Add(f2VertexTopLeft);
			oVertices.Add(f2VertexTopRight);
			oVertices.Add(f2VertexBottomRight);
			
			return oVertices;
		}
		
		public Rect GetRect()
		{
			return new Rect(x - width * 0.5f, y - height * 0.5f, width, height);
		}
		
		public bool Contains(Vector2 a_f2Point)
		{
			return GetRect().Contains(a_f2Point);
		}
		
		public Rectangle()
		{
		}
		
		public Rectangle(float a_fX, float a_fY, float a_fWidth, float a_fHeight)
		{
			x = a_fX;
			y = a_fY;
			width = a_fWidth;
			height = a_fHeight;
		}
		
		// --- World Transform ---
		
		public bool Raycast(Transform a_rTransform, Ray a_oRay)
		{
			float fHitDistanceAlongTheRay;
			Vector3 f3HitPoint;
			return Raycast(a_rTransform, a_oRay, out f3HitPoint, out fHitDistanceAlongTheRay);
		}
		
		public bool Raycast(Transform a_rTransform, Ray a_oRay, out Vector3 a_f3HitPoint)
		{
			float fHitDistanceAlongTheRay;
			return Raycast(a_rTransform, a_oRay, out a_f3HitPoint, out fHitDistanceAlongTheRay);
		}
		
		public bool Raycast(Transform a_rTransform, Ray a_oRay, out float a_fHitDistanceAlongTheRay)
		{
			Vector3 f3HitPoint;
			return Raycast(a_rTransform, a_oRay, out f3HitPoint, out a_fHitDistanceAlongTheRay);
		}
		
		public bool Raycast(Transform a_rTransform, Ray a_oRay, out Vector3 a_f3HitPoint, out float a_fHitDistanceAlongTheRay)
		{
			a_f3HitPoint = Vector3.zero;
			
			Plane oRectanglePlane = ComputeRectanglePlane(a_rTransform);
			if(oRectanglePlane.Raycast(a_oRay, out a_fHitDistanceAlongTheRay))
			{
				a_f3HitPoint = a_oRay.origin + a_oRay.direction * a_fHitDistanceAlongTheRay;
				if(Contains(a_rTransform, a_f3HitPoint))
				{
					return true;
				}
			}
			return false;
		}
		
		public Plane ComputeRectanglePlane(Transform a_rTransform)
		{
			Plane oRectanglePlane;
			oRectanglePlane = new Plane(-a_rTransform.forward, a_rTransform.position);
			
			return oRectanglePlane;
		}
		
		public bool Contains(Transform a_rTransform, Vector3 a_f3Point)
		{
			Vector2 f2PointProjection = ProjectIntoLocalSpace(a_rTransform, a_f3Point);
			return Contains(f2PointProjection);
		}
		
		public bool Contains(Transform a_rTransform, Camera a_rCamera, Vector2 a_f2ScreenPoint)
		{
			Ray oRay = a_rCamera.ScreenPointToRay(a_f2ScreenPoint);
			return Raycast(a_rTransform, oRay);
		}

		public bool Project(Transform a_rTransform, Camera a_rCamera, Vector2 a_f2ScreenPoint, out Vector3 projectionInWorldSpace)
		{
			Ray oRay = a_rCamera.ScreenPointToRay(a_f2ScreenPoint);
			return Raycast(a_rTransform, oRay, out projectionInWorldSpace);
		}

		public List<Vector3> GetWorldVertices(Transform a_rTransform)
		{
			List<Vector3> oLocalVertices = GetLocalVertices();
			List<Vector3> oVertices = new List<Vector3>();
			foreach(Vector3 f3VertexInLocalSpace in oLocalVertices)
			{
				Vector3 f3Vertex = TransformLocal2DPointIntoWorldSpace(a_rTransform, f3VertexInLocalSpace);
				oVertices.Add(f3Vertex);
			}
			
			return oVertices;
		}
		
		public void DisplayLinesGizmos(Transform a_rTransform, Color a_oColorLine)
		{
			Color oColorSave = Gizmos.color;
			Gizmos.color = a_oColorLine;
			
			List<Vector3> oVertices = GetWorldVertices(a_rTransform);
			int iVertexCount = oVertices.Count;
			for(int i = 0; i < iVertexCount; i++)
			{
				Vector3 f3VertexA = oVertices[i];
				Vector3 f3VertexB = oVertices[(i+1)%iVertexCount];
				
				Gizmos.DrawLine(f3VertexA, f3VertexB);
			}
			
			Gizmos.color = oColorSave;
		}
		
		Vector2 ProjectIntoLocalSpace(Transform a_rTransform, Vector3 a_f3PointInWorldSpace)
		{		
			Vector3 f3PointInLocalSpace = a_rTransform.InverseTransformPoint(a_f3PointInWorldSpace);
			
			Vector2 f2ProjectedPointInLocalSpace = new Vector2(f3PointInLocalSpace.x, f3PointInLocalSpace.y);
			
			return f2ProjectedPointInLocalSpace;
		}
		
		Vector3 TransformLocal2DPointIntoWorldSpace(Transform a_rTransform, Vector2 a_f2PointInLocalSpace)
		{
			Vector3 f3PointInLocalSpace = new Vector3(a_f2PointInLocalSpace.x, a_f2PointInLocalSpace.y, 0.0f);
			
			if(a_rTransform == null)
			{
				return f3PointInLocalSpace;	
			}
			
			Vector3 f3PointInWorldSpace = a_rTransform.TransformPoint(f3PointInLocalSpace);
			
			return f3PointInWorldSpace;
		}
	
		// --- Screen Aligned transform ---
		
		public Rect GetScreenAlignedRectangle(Transform a_rTransform, Camera a_rCamera)
		{
			Vector3 f3Center;
			Vector2 f2Size;
			GetScreenAlignedTransformInfos(a_rTransform, ref a_rCamera, out f3Center, out f2Size);
			
			Rect oRect = GetRect();
			oRect.width *= f2Size.x;
			oRect.height *= f2Size.y;
			oRect.x = f3Center.x - oRect.width * 0.5f;
			oRect.y = (Screen.height - f3Center.y) - oRect.height * 0.5f;
			
			return oRect;
		}
		
		public List<Vector3> GetScreenAlignedVertices(Transform a_rTransform, Camera a_rCamera)
		{	
			Vector3 f3Center;
			Vector2 f2Size;
			GetScreenAlignedTransformInfos(a_rTransform, ref a_rCamera, out f3Center, out f2Size);
			
			List<Vector3> oLocalVertices = GetLocalVertices();
			List<Vector3> oVertices = new List<Vector3>();
			foreach(Vector3 f3VertexInLocalSpace in oLocalVertices)
			{
				Vector3 f3Vertex = TransformLocal2DPointIntoScreenAlignedWorldSpace(a_rCamera, f3VertexInLocalSpace, f3Center, f2Size);
				oVertices.Add(f3Vertex);
			}
			
			return oVertices;
		}
		
		public void DisplayScreenAlignedLinesGizmos(Transform a_rTransform, Camera a_rCamera, Color a_oColorLine)
		{
			Color oColorSave = Gizmos.color;
			Gizmos.color = a_oColorLine;
			
			List<Vector3> oVertices = GetScreenAlignedVertices(a_rTransform, a_rCamera);
			int iVertexCount = oVertices.Count;
			for(int i = 0; i < iVertexCount; i++)
			{
				Vector3 f3VertexA = oVertices[i];
				Vector3 f3VertexB = oVertices[(i+1)%iVertexCount];
				
				Gizmos.DrawLine(f3VertexA, f3VertexB);
			}
			
			Gizmos.color = oColorSave;
		}
		
		Vector3 TransformLocal2DPointIntoScreenAlignedWorldSpace(Camera a_rCamera, Vector3 a_f3PointInLocalSpace, Vector3 a_f3OriginInScreenSpace, Vector2 a_f2Scale)
		{
			Vector3 f3PointInLocalSpace_Scaled = a_f3PointInLocalSpace;
			f3PointInLocalSpace_Scaled.x *= a_f2Scale.x;
			f3PointInLocalSpace_Scaled.y *= a_f2Scale.y;
			
			Vector3 f3PointInScreenSpace = a_f3OriginInScreenSpace + f3PointInLocalSpace_Scaled;
			
			Vector3 f3PointInWorldSpace = a_rCamera.ScreenToWorldPoint(f3PointInScreenSpace);
			
			return f3PointInWorldSpace;
		}
		
		void GetScreenAlignedTransformInfos(Transform a_rTransform, ref Camera a_rCamera, out Vector3 a_f3Center, out Vector2 a_f2Size)
		{
			if(a_rCamera == null)
			{
				a_rCamera = Camera.main;
			}
			
			Vector3 f3OriginWorldPosition = a_rTransform.position;
			
			Vector3 f3OriginInScreenSpace = a_rCamera.WorldToScreenPoint(f3OriginWorldPosition);
			
			Vector3 f3RightFromOriginWorldPosition = a_rCamera.ScreenToWorldPoint(f3OriginInScreenSpace + Vector3.right);
			Vector3 f3UpFromOriginWorldPosition = a_rCamera.ScreenToWorldPoint(f3OriginInScreenSpace + Vector3.up);
			
			Vector3 f3Scale = a_rTransform.lossyScale;
			f3Scale.x /= (f3RightFromOriginWorldPosition - f3OriginWorldPosition).magnitude;
			f3Scale.y /= (f3UpFromOriginWorldPosition - f3OriginWorldPosition).magnitude;
			
			a_f3Center = f3OriginInScreenSpace;
			a_f2Size = f3Scale;
		}
		
		//--- Viewport aligned ---
		
		public Rect GetViewportAlignedRectangle(Transform a_rTransform, Camera a_rCamera)
		{
			Vector3 f3Center;
			Vector2 f2Size;
			GetViewportAlignedTransformInfos(a_rTransform, ref a_rCamera, out f3Center, out f2Size);
			
			Rect oRect = GetRect();
			oRect.width *= f2Size.x;
			oRect.height *= f2Size.y;
			oRect.x = f3Center.x - oRect.width * 0.5f;
			oRect.y = f3Center.y - oRect.height * 0.5f;
			
			return oRect;
		}
		
		void GetViewportAlignedTransformInfos(Transform a_rTransform, ref Camera a_rCamera, out Vector3 a_f3Center, out Vector2 a_f2Size)
		{
			if(a_rCamera == null)
			{
				a_rCamera = Camera.main;
			}
			
			Vector3 f3OriginWorldPosition = a_rTransform.position;
			
			Vector3 f3OriginInViewportSpace = a_rCamera.WorldToViewportPoint(f3OriginWorldPosition);
			
			Vector3 f3RightFromOriginWorldPosition = a_rCamera.ViewportToWorldPoint(f3OriginInViewportSpace + Vector3.right);
			Vector3 f3UpFromOriginWorldPosition = a_rCamera.ViewportToWorldPoint(f3OriginInViewportSpace + Vector3.up);
			
			Vector3 f3Scale = a_rTransform.lossyScale;
			f3Scale.x /= (f3RightFromOriginWorldPosition - f3OriginWorldPosition).magnitude;
			f3Scale.y /= (f3UpFromOriginWorldPosition - f3OriginWorldPosition).magnitude;
			
			a_f3Center = f3OriginInViewportSpace;
			a_f2Size = f3Scale;
		}
	}
}