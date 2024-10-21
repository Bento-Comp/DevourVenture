using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[System.Serializable]
	public class Polygon2DWithTransform
	{
		public Transform polygonXYPlaneTransform;
		
		public Polygon2D polygon2D;
		
		public int VertexCount
		{
			get
			{
				return polygon2D.VertexCount;
			}
		}
		
		public bool Overlaps(Polygon2DWithTransform a_rPolygonWithTransform, Camera a_rProjectionCamera = null)
		{
			return polygon2D.Overlaps(GetPolygonVerticesInLocalSpace(a_rPolygonWithTransform, a_rProjectionCamera));
		}
		
		public Vector3 ComputeLocalPushVectorToKeepInsideBounds(Polygon2DWithTransform a_rPolygonWithTransform, Camera a_rProjectionCamera = null)
		{
			return polygon2D.ComputePushVectorToKeepInsideBounds(GetPolygonVerticesInLocalSpace(a_rPolygonWithTransform, a_rProjectionCamera));
		}
		
		public Vector3 ComputeLocalPushVectorToKeepInsideBounds(Polygon2DWithTransform a_rPolygonWithTransform, Vector2 a_f2Thickness, Camera a_rProjectionCamera = null)
		{
			return polygon2D.ComputePushVectorToKeepInsideBounds(GetPolygonVerticesInLocalSpace(a_rPolygonWithTransform, a_rProjectionCamera), a_f2Thickness);
		}
		
		public Vector3 ComputePushVectorAlongALocalDirectionToGetInsidePolygon(Vector3 a_f3Direction, Polygon2DWithTransform a_rPolygonWithTransform, Camera a_rProjectionCamera = null)
		{
			return polygon2D.ComputePushVectorAlongDirectionToGetInsidePolygon(a_f3Direction, GetPolygonVerticesInLocalSpace(a_rPolygonWithTransform, a_rProjectionCamera));
		}
		
		public Vector3 ComputeLocalMin()
		{
			return polygon2D.ComputeMin();
		}
		
		public Vector3 ComputeLocalMax()
		{
			return polygon2D.ComputeMax();
		}
		
		public float ComputeLocalWidth()
		{
			return polygon2D.ComputeWidth();
		}
		
		public float ComputeLocalHeight()
		{
			return polygon2D.ComputeHeight();
		}
		
		public Vector2 ComputeLocalSize()
		{
			return polygon2D.ComputeSize();
		}
		
		public bool Raycast(Ray a_oRay)
		{
			float fHitDistanceAlongTheRay;
			Vector3 f3HitPoint;
			return Raycast(a_oRay, out f3HitPoint, out fHitDistanceAlongTheRay);
		}
		
		public bool Raycast(Ray a_oRay, out Vector3 a_f3HitPoint)
		{
			float fHitDistanceAlongTheRay;
			return Raycast(a_oRay, out a_f3HitPoint, out fHitDistanceAlongTheRay);
		}
		
		public bool Raycast(Ray a_oRay, out float a_fHitDistanceAlongTheRay)
		{
			Vector3 f3HitPoint;
			return Raycast(a_oRay, out f3HitPoint, out a_fHitDistanceAlongTheRay);
		}
		
		public bool Raycast(Ray a_oRay, out Vector3 a_f3HitPoint, out float a_fHitDistanceAlongTheRay)
		{
			if(RaycastPolygonPlane(a_oRay, out a_f3HitPoint, out a_fHitDistanceAlongTheRay))
			{
				if(ContainsAsConvex(a_f3HitPoint))
				{
					return true;
				}
			}
			return false;
		}
		
		public bool RaycastPerimeter(Ray a_rRay_LocalToReferential, Transform a_rReferential, out Vector3 a_f3HitPoint)
		{
			a_f3HitPoint = Vector3.zero;
			
			// Compute ray local to polygon
			Vector3 f3Origin_LocalToPolygon = polygonXYPlaneTransform.InverseTransformPoint(a_rReferential.TransformPoint(a_rRay_LocalToReferential.origin));
			Vector3 f3Direction_LocalToPolygon = polygonXYPlaneTransform.InverseTransformDirection(a_rReferential.TransformDirection(a_rRay_LocalToReferential.direction));
			Ray oRay_LocalToPolygon = new Ray(f3Origin_LocalToPolygon, f3Direction_LocalToPolygon); 
			
			// Ray cast
			Vector2 f2HitPoint_LocalToPolygon;
			if(polygon2D.RaycastPerimeter(oRay_LocalToPolygon, out f2HitPoint_LocalToPolygon))
			{
				a_f3HitPoint = a_rReferential.InverseTransformPoint(polygonXYPlaneTransform.TransformPoint(f2HitPoint_LocalToPolygon));
				return true;
			}
			return false;
		}
		
		public bool RaycastPolygonPlane(Ray a_oRay, out Vector3 a_f3HitPoint)
		{
			float fHitDistanceAlongTheRay;
			return RaycastPolygonPlane(a_oRay, out a_f3HitPoint, out fHitDistanceAlongTheRay);
		}
		
		public bool RaycastPolygonPlane(Ray a_oRay, out float a_fHitDistanceAlongTheRay)
		{
			Vector3 f3HitPoint;
			return RaycastPolygonPlane(a_oRay, out f3HitPoint, out a_fHitDistanceAlongTheRay);
		}
		
		public bool RaycastPolygonPlane(Ray a_oRay, out Vector3 a_f3HitPoint, out float a_fHitDistanceAlongTheRay)
		{
			a_f3HitPoint = Vector3.zero;
			
			Plane oPolygonPlane;
			if(polygonXYPlaneTransform == null)
			{
				oPolygonPlane = new Plane(Vector3.back, Vector3.zero);
			}
			else
			{
				oPolygonPlane = new Plane(-polygonXYPlaneTransform.forward, polygonXYPlaneTransform.position);
			}
			if(oPolygonPlane.Raycast(a_oRay, out a_fHitDistanceAlongTheRay))
			{
				a_f3HitPoint = a_oRay.origin + a_oRay.direction * a_fHitDistanceAlongTheRay;
				return true;
			}
			return false;
		}
		
		public bool ContainsAsConvex(Vector2 a_f2ScreenPoint, Camera a_rScreenCamera)
		{
			Ray oRay = a_rScreenCamera.ScreenPointToRay(a_f2ScreenPoint);
			return Raycast(oRay);
		}
		
		public bool ContainsAsConvex(Vector3 a_f3WorldPoint, Camera a_rProjectionCamera = null)
		{
			if(polygon2D == null)
			{
				return false;
			}
			
			Vector2 f2PointProjection = ProjectIntoLocalSpace(a_f3WorldPoint, a_rProjectionCamera);
			return polygon2D.ContainsAsConvex(f2PointProjection);
		}
		
		public bool ContainsAsConvex(List<Vector3> a_f3Points, Camera a_rProjectionCamera = null)
		{
			// Must contains all the points
			foreach(Vector3 f3Point in a_f3Points)
			{
				if(ContainsAsConvex(f3Point, a_rProjectionCamera) == false)
				{
					return false;
				}
			}
			return true;
		}
		
		public bool ContainsAtLeastAPointAsConvex(List<Vector3> a_f3Points, Camera a_rProjectionCamera = null)
		{
			// Must contains all the points
			foreach(Vector3 f3Point in a_f3Points)
			{
				if(ContainsAsConvex(f3Point, a_rProjectionCamera))
				{
					return true;
				}
			}
			return false;
		}
		
		public bool ContainsAsConvex(Polygon2DWithTransform a_rPolygon2DWithTransform, Camera a_rProjectionCamera = null)
		{
			List<Vector3> oVerticesInWorldSpace = a_rPolygon2DWithTransform.GetVertices();
			return ContainsAsConvex(oVerticesInWorldSpace, a_rProjectionCamera);
		}
		 
		public bool ContainsAtLeastAPointAsConvex(Polygon2DWithTransform a_rPolygon2DWithTransform, Camera a_rProjectionCamera = null)
		{
			List<Vector3> oVerticesInWorldSpace = a_rPolygon2DWithTransform.GetVertices();
			return ContainsAtLeastAPointAsConvex(oVerticesInWorldSpace, a_rProjectionCamera);
		}
		
		public List<Vector2> GetPolygonVerticesInLocalSpace(Polygon2DWithTransform a_rOtherPolygonWithTransform, Camera a_rProjectionCamera = null)
		{
			List<Vector2> oOtherPolygonVerticesInLocalSpace = new List<Vector2>();
			List<Vector3> oOtherPolygonVertices = a_rOtherPolygonWithTransform.GetVertices();
			foreach(Vector3 f3OtherPolygonVertex in oOtherPolygonVertices)
			{
				Vector3 f2OtherPolygonVertexInLocalSpace = ProjectIntoLocalSpace(f3OtherPolygonVertex, a_rProjectionCamera);
				oOtherPolygonVerticesInLocalSpace.Add(f2OtherPolygonVertexInLocalSpace);
			}
			
			return oOtherPolygonVerticesInLocalSpace;
		}
		
		public List<Vector3> GetVertices()
		{
			List<Vector3> oVertices = new List<Vector3>();
			foreach(Vector2 f2VertexInLocalSpace in polygon2D.Vertices)
			{
				Vector3 f3Vertex = TransformLocal2DPointIntoWorldSpace(f2VertexInLocalSpace);
				oVertices.Add(f3Vertex);
			}
			
			return oVertices;
		}
		
		public Vector3 GetVertex(int a_iVertexIndex)
		{
			Vector2 f2VertexInLocalSpace = polygon2D.Vertices[a_iVertexIndex];
			Vector3 f3Vertex = TransformLocal2DPointIntoWorldSpace(f2VertexInLocalSpace);
			
			return f3Vertex;
		}
		
		public void ClearVertices()
		{
			polygon2D.Clear();
		}
		
		public void AddVertex(Vector3 a_f3VertexInRelativeSpace, Transform a_rRelativeSpaceTransform)
		{
			Vector3 f3VertexInWorldSpace;
			if(a_rRelativeSpaceTransform == null)
			{
				f3VertexInWorldSpace = a_f3VertexInRelativeSpace;
			}
			else
			{
				f3VertexInWorldSpace = a_rRelativeSpaceTransform.TransformPoint(a_f3VertexInRelativeSpace);
			}
			AddVertex(f3VertexInWorldSpace);
		}
		
		public void AddVertex(Vector3 a_f3VertexInWorldSpace)
		{
			if(polygon2D == null)
			{
				polygon2D = new Polygon2D();	
			}
			
			Vector2 f2PolygonVertex = ProjectIntoLocalSpace(a_f3VertexInWorldSpace);
			polygon2D.AddVertex(f2PolygonVertex);
		}
		
		public void AddLocalVertex(Vector2 a_f2VertexInLocalSpace)
		{
			if(polygon2D == null)
			{
				polygon2D = new Polygon2D();	
			}
			
			polygon2D.AddVertex(a_f2VertexInLocalSpace);
		}
		
		public Vector3 ProjectLocalPositionIntoPolygonPlane(Vector3 a_f3LocalPosition, Camera a_rProjectionCamera = null)
		{
			if(polygonXYPlaneTransform == null)
			{
				return a_f3LocalPosition;
			}
			else
			{
				return ProjectIntoLocalSpace(polygonXYPlaneTransform.TransformPoint(a_f3LocalPosition), a_rProjectionCamera);
			}
		}
		
		public Vector2 ProjectIntoLocalSpace(Vector3 a_f3PointInWorldSpace, Camera a_rProjectionCamera = null)
		{
			if(polygonXYPlaneTransform == null)
			{
				return a_f3PointInWorldSpace;
			}
			
			if(a_rProjectionCamera != null)
			{
				Vector2 f2ScreenPoint = a_rProjectionCamera.WorldToScreenPoint(a_f3PointInWorldSpace);
				Ray oRay = a_rProjectionCamera.ScreenPointToRay(f2ScreenPoint);
				RaycastPolygonPlane(oRay, out a_f3PointInWorldSpace);		
			}
			Vector3 f3PointInLocalSpace = polygonXYPlaneTransform.InverseTransformPoint(a_f3PointInWorldSpace);
			
			Vector2 f2ProjectedPointInLocalSpace = new Vector2(f3PointInLocalSpace.x, f3PointInLocalSpace.y);
			
			return f2ProjectedPointInLocalSpace;
		}
		
		public void DisplayGizmos(bool a_bDrawLines, bool a_bDrawVertices, bool a_bDrawTransform, Color a_oColorLine, Color a_oColorVertex, float a_fVerticeSize)
		{
			if(a_bDrawTransform)
			{
				DisplayTransformGizmos();	
			}
			
			if(a_bDrawLines)
			{
				DisplayLinesGizmos(a_oColorLine);
			}
			
			if(a_bDrawVertices)
			{
				DisplayVerticesGizmos(a_oColorVertex, a_fVerticeSize);
			}
		}
		
		public void DisplayTransformGizmos()
		{
			if(polygonXYPlaneTransform != null)
			{
				Vector3 f3Origin = polygonXYPlaneTransform.position;
				Vector3 f3Right = polygonXYPlaneTransform.right;
				Vector3 f3Up = polygonXYPlaneTransform.up;
				Vector3 f3Forward = polygonXYPlaneTransform.forward;
			
				DrawNormalGizmo(f3Origin, f3Right, Color.red);
				DrawNormalGizmo(f3Origin, f3Up, Color.green);
				DrawNormalGizmo(f3Origin, f3Forward, Color.blue);
			}
		}
		
		public void DisplayLinesGizmos(Color a_oColorLine)
		{
			Color oColorSave = Gizmos.color;
			Gizmos.color = a_oColorLine;
			
			List<Vector3> oVertices = GetVertices();
			int iVertexCount = oVertices.Count;
			for(int i = 0; i < iVertexCount; i++)
			{
				Vector3 f3VertexA = oVertices[i];
				Vector3 f3VertexB = oVertices[(i+1)%iVertexCount];
				
				Gizmos.DrawLine(f3VertexA, f3VertexB);
			}
			
			Gizmos.color = oColorSave;
		}
		
		public void DisplayVerticesGizmos(Color a_oColorVertex, float a_fVerticeSize = 0.01f)
		{
			Color oColorSave = Gizmos.color;
			Gizmos.color = a_oColorVertex;
			
			List<Vector3> oVertices = GetVertices();
			int iVertexCount = oVertices.Count;
			for(int i = 0; i < iVertexCount; i++)
			{
				Vector3 f3VertexA = oVertices[i];
	
				Gizmos.DrawSphere(f3VertexA, a_fVerticeSize);
			}
			
			Gizmos.color = oColorSave;
		}
		
		public Polygon2DWithTransform()
		{
			if(polygon2D == null)
			{
				polygon2D = new Polygon2D();	
			}
		}
		
		public Polygon2DWithTransform(EPolygonPrimitive a_ePrimitive)
		{
			if(polygon2D == null)
			{
				polygon2D = new Polygon2D(a_ePrimitive);
			}
		}
		
		Vector3 TransformLocal2DPointIntoWorldSpace(Vector2 a_f2PointInLocalSpace)
		{
			Vector3 f3PointInLocalSpace = new Vector3(a_f2PointInLocalSpace.x, a_f2PointInLocalSpace.y, 0.0f);
			
			if(polygonXYPlaneTransform == null)
			{
				return f3PointInLocalSpace;	
			}
			
			Vector3 f3PointInWorldSpace = polygonXYPlaneTransform.TransformPoint(f3PointInLocalSpace);
			
			return f3PointInWorldSpace;
		}
		
		void DrawNormalGizmo(Vector3 a_f3Position, Vector3 a_f3Normal, Color a_oColor)
		{
			Color oColorSave = Gizmos.color;
			Gizmos.color = a_oColor;
			Gizmos.DrawLine(a_f3Position, a_f3Position + a_f3Normal);
			Gizmos.color = oColorSave;
		}
	}
}
