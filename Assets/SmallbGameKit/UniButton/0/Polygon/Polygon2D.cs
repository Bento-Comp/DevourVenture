using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	public enum EPolygonPrimitive
	{
		Square
	}
	
	[System.Serializable]
	public class Polygon2D
	{
		[SerializeField]
		List<Vector2> vertices;
		
		public List<Vector2> Vertices
		{
			get
			{
				return vertices;
			}
		}
		
		public int VertexCount
		{
			get
			{
				return vertices.Count;
			}
		}
		
		public bool Overlaps(List<Vector2> a_rVertices)
		{
			PolygonCollisionResult r = PolygonCollision(vertices, a_rVertices, Vector2.zero);
			
			return r.Intersect;
		}
		
		public Vector2 ComputePushVectorToKeepInsideBounds(List<Vector2> a_rVertices, Vector2 a_f2SkinThickness)
		{
			Vector2 f2Min = ComputeMin();
			Vector2 f2Max = ComputeMax();
			
			return ComputePushVectorToKeepInsideBounds(a_rVertices, f2Min, f2Max, a_f2SkinThickness);
		}
		
		public Vector2 ComputePushVectorToKeepInsideBounds(List<Vector2> a_rVertices)
		{
			Vector2 f2Min = ComputeMin();
			Vector2 f2Max = ComputeMax();
			
			return ComputePushVectorToKeepInsideBounds(a_rVertices, f2Min, f2Max);
		}
		
		public static Vector2 ComputePushVectorToKeepInsideBounds(List<Vector2> a_rVertices, Vector2 a_f2Min, Vector2 a_f2Max, Vector2 a_f2SkinThickness)
		{
			Vector2 f2MinThickened = a_f2Min - a_f2SkinThickness;
			Vector2 f2MaxThickened = a_f2Max + a_f2SkinThickness;
			
			Vector2 f2Min = Vector2.Min(f2MinThickened, f2MaxThickened);
			Vector2 f2Max = Vector2.Max(f2MinThickened, f2MaxThickened);
			
			return ComputePushVectorToKeepInsideBounds(a_rVertices, f2Min, f2Max);
		}
		
		public static Vector2 ComputePushVectorToKeepInsideBounds(List<Vector2> a_rVertices, Vector2 a_f2Min, Vector2 a_f2Max)
		{
			Vector2 f2PushMax = Vector2.zero;	
			foreach(Vector2 f2Vertex in a_rVertices)
			{
				Vector2 f2Push = ComputePushVectorToKeepInsideBounds(f2Vertex, a_f2Min, a_f2Max);
			
				if(Mathf.Abs(f2Push.x) > Mathf.Abs(f2PushMax.x))
				{
					f2PushMax.x = f2Push.x;
				}
				
				if(Mathf.Abs(f2Push.y) > Mathf.Abs(f2PushMax.y))
				{
					f2PushMax.y = f2Push.y;
				}
			}
			
			return f2PushMax;
		}
		
		public static Vector2 ComputePushVectorToKeepInsideBounds(Vector2 a_f2Vertex, Vector2 a_f2Min, Vector2 a_f2Max)
		{	
			Vector2 f2Push = Vector3.zero;
			
			// x
			if(a_f2Vertex.x < a_f2Min.x)
			{
				f2Push.x = a_f2Min.x - a_f2Vertex.x;
			}
			else if(a_f2Vertex.x > a_f2Max.x)
			{
				f2Push.x = a_f2Max.x - a_f2Vertex.x;
			}
			
			// y
			if(a_f2Vertex.y < a_f2Min.y)
			{
				f2Push.y = a_f2Min.y - a_f2Vertex.y;
			}
			else if(a_f2Vertex.y > a_f2Max.y)
			{
				f2Push.y = a_f2Max.y - a_f2Vertex.y;
			}
			
			return f2Push;
		}
		
		public Vector2 ComputePushVectorAlongDirectionToGetInsidePolygon(Vector2 a_f2Direction, List<Vector2> a_rVertices)
		{
			float fPushMax = 0.0f;
			foreach(Vector2 f2Vertex in a_rVertices)
			{
				float fPush = ComputePushDistanceAlongADirectionToGetInsidePolygon(a_f2Direction, f2Vertex);
			
				if(Mathf.Abs(fPush) > Mathf.Abs(fPushMax))
				{
					fPushMax = fPush;
				}
			}
			
			return fPushMax * a_f2Direction;
		}
		
		public float ComputePushDistanceAlongADirectionToGetInsidePolygon(Vector2 a_f2Direction, Vector2 a_f2Vertex)
		{
			if(ContainsAsConvex(a_f2Vertex))
			{
				return 0.0f;
			}
			
			// Raycast forward
			Ray oRayForward = new Ray(a_f2Vertex, a_f2Direction);
			float fRaycastDistanceForward;
			if(Raycast(oRayForward, out fRaycastDistanceForward))
			{
				return fRaycastDistanceForward;
			}
			
			// Raycast backward
			/*Ray oRayBackward = new Ray(a_f2Vertex, -a_f2Direction);
			float fRaycastDistanceBackward;
			if(Raycast(oRayBackward, out fRaycastDistanceBackward))
			{
				return -fRaycastDistanceBackward;
			}*/
			
			return 0.0f;
		}
		
		public Vector2 ComputeMin()
		{
			if(vertices.Count <= 0)
			{
				return Vector3.zero;
			}
			
			Vector2 f2Min = Vector2.one * float.PositiveInfinity; 
			foreach(Vector2 f2Vertex in vertices)
			{
				f2Min = Vector2.Min(f2Vertex, f2Min);
			}
			
			return f2Min;
		}
		
		public Vector2 ComputeMax()
		{
			if(vertices.Count <= 0)
			{
				return Vector3.zero;
			}
			
			Vector2 f2Max = Vector2.one * float.NegativeInfinity; 
			foreach(Vector2 f2Vertex in vertices)
			{
				f2Max = Vector2.Max(f2Vertex, f2Max);
			}
			
			return f2Max;
		}
		
		public float ComputeWidth()
		{
			return ComputeMax().x - ComputeMin().x;
		}
		
		public float ComputeHeight()
		{
			return ComputeMax().y - ComputeMin().y;
		}
		
		public Vector2 ComputeSize()
		{
			Vector2 f2Size;
			
			Vector2 f2Max = ComputeMax();
			Vector2 f2Min = ComputeMin();
			
			f2Size.x = f2Max.x - f2Min.x;
			f2Size.y = f2Max.y - f2Min.y;
			
			return f2Size;
		}
		
		public void Clear()
		{
			vertices.Clear();
		}
		
		public void AddVertex(Vector2 a_f2Vertex)
		{
			if(vertices == null)
			{
				vertices = new List<Vector2>();
			}
			vertices.Add(a_f2Vertex);
		}
		
		public bool ContainsAsConvex(Vector2 a_f2Point, float a_fTolerance = 0.0f)
		{
			// Loop through the edges
			int iVertexCount = vertices.Count;
			for(int i = 0; i < iVertexCount; i++)
			{
				Vector2 f2SegmentBegin = vertices[i];
				Vector2 f2SegmentEnd = vertices[(i+1)%iVertexCount];
				
				// If the point is in the outer half space we know is not in the convex polygon 
				if(IsInHalfSpace(f2SegmentEnd, f2SegmentBegin, a_f2Point, a_fTolerance) == false)
				{
					return false;
				}
			}
			
			// If we reached the point is in all the half space
			// i.e. in the convex polygon
			return true;
		}
		
		static public bool IsInHalfSpace(Vector2 a_f2SegmentBegin, Vector2 a_f2SegmentEnd, Vector2 a_f2Point, float a_fTolerance = 0.0f)
		{
			Vector2 f2SegmentDirection = a_f2SegmentEnd - a_f2SegmentBegin;
			Vector2 f2SegmentBeginToPoint = a_f2Point - a_f2SegmentBegin;
			
			float fCrossProduct = f2SegmentDirection.x * f2SegmentBeginToPoint.y - f2SegmentDirection.y * f2SegmentBeginToPoint.x;
			if(fCrossProduct > 0.0f)
			{
				return true;
			}
			else if(a_fTolerance > 0.0f)
			{
				if(DistancePointToSegment(a_f2SegmentBegin, a_f2SegmentEnd, a_f2Point) <= a_fTolerance)
				{
					return true;
				}
			}
			
			return false;
		}
		
		public float DistancePointToPolygon(Vector2 a_f2Point)
		{
			Vector2 f2DistanceToPolygon;
			return DistancePointToPolygon(a_f2Point, out f2DistanceToPolygon);
		}
		
		public float DistancePointToPolygon(Vector2 a_f2Point, out Vector2 a_f2Distance)
		{	
			return DistancePointToConvexBounds(a_f2Point, vertices, out a_f2Distance);
		}
		
		public static float DistancePointToConvexBounds(Vector2 a_f2Point, List<Vector2> a_f2Vertices, out Vector2 a_f2Distance)
		{
			Vector2 f2DistanceMin = Vector3.zero;
			float fDistanceMin = float.PositiveInfinity;
			
			bool bOutFromAtLeastOneHalfSpace = false;
			
			// Loop through the edges
			int iVertexCount = a_f2Vertices.Count;
			for(int i = 0; i < iVertexCount; i++)
			{
				Vector2 f2SegmentBegin = a_f2Vertices[i];
				Vector2 f2SegmentEnd = a_f2Vertices[(i+1)%iVertexCount];
				
				// If the point is in the outer half space of a segment we look its distance from it 
				if(IsInHalfSpace(f2SegmentBegin, f2SegmentEnd, a_f2Point))
				{
					bOutFromAtLeastOneHalfSpace = true;
					
					Vector2 f2Distance;
					float fDistance = DistancePointToSegment(f2SegmentBegin, f2SegmentEnd, a_f2Point, out f2Distance);
					if(fDistance < fDistanceMin)
					{
						fDistanceMin = fDistance;
						f2DistanceMin = f2Distance;
					}
				}
			}
			
			if(bOutFromAtLeastOneHalfSpace)
			// If we are out of the polygon
			{
				a_f2Distance = f2DistanceMin;
				return fDistanceMin;
			}
			else
			// Else we are in the polygon
			{
				a_f2Distance = Vector3.zero;
				return 0.0f;
			}
		}
		
		static public float DistancePointToSegment(Vector2 a_f2SegmentBegin, Vector2 a_f2SegmentEnd, Vector2 a_f2Point)
		{
			Vector2 f2Distance;
			return DistancePointToSegment(a_f2SegmentBegin, a_f2SegmentEnd, a_f2Point, out f2Distance);
		}
		
		static public float DistancePointToSegment(Vector2 a_f2SegmentBegin, Vector2 a_f2SegmentEnd, Vector2 a_f2Point, out Vector2 a_f2Distance)
		{
			a_f2Distance = DistanceVectorPointToSegment(a_f2SegmentBegin, a_f2SegmentEnd, a_f2Point);
			
			return a_f2Distance.magnitude;
		}
		
		static public Vector2 DistanceVectorPointToSegment(Vector2 a_f2SegmentBegin, Vector2 a_f2SegmentEnd, Vector2 a_f2Point)
		{
			Vector2 f2SegmentDirection = a_f2SegmentEnd - a_f2SegmentBegin;
			Vector2 f2SegmentBeginToPoint = a_f2Point - a_f2SegmentBegin;
			
			float fSegmentSquareLength = f2SegmentDirection.sqrMagnitude;
			
			if((double)fSegmentSquareLength == 0.0)
			{
				return -f2SegmentBeginToPoint;
			}
			
			float fDotPercent = Vector2.Dot(f2SegmentBeginToPoint, f2SegmentDirection) / fSegmentSquareLength;
			double dDotPercent = (double)fDotPercent;
			if(dDotPercent < 0.0)
			{
				return -f2SegmentBeginToPoint;
			}
			
			if(dDotPercent > 1.0)
			{
				return a_f2SegmentEnd - a_f2Point;
			}
			
			return (a_f2SegmentBegin + fDotPercent * f2SegmentDirection) - a_f2Point;
		}
		
		public bool Raycast(Ray ray, out Vector2 a_f2HitPoint, float tmax = float.PositiveInfinity) 
	    {
			Vector2 f2Normal;
			float fDistanceAlongTheRay;
			return Raycast(ray, out fDistanceAlongTheRay, out a_f2HitPoint, out f2Normal, tmax);
		}
		
		public bool Raycast(Ray ray, out float t, float tmax = float.PositiveInfinity) 
	    {
			Vector2 f2HitPoint;
			Vector2 f2Normal;
			return Raycast(ray, out t, out f2HitPoint, out f2Normal, tmax);
		}
		
		public bool RaycastPerimeter(Ray ray, out Vector2 a_f2HitPoint, float tmax = float.PositiveInfinity) 
	    {
			Vector2 f2Normal;
			float fDistanceAlongTheRay;
			return RaycastPerimeter(ray, out fDistanceAlongTheRay, out a_f2HitPoint, out f2Normal, tmax);
		}
		
		public bool RaycastPerimeter(Ray ray, out float t, float tmax = float.PositiveInfinity) 
	    {
			Vector2 f2HitPoint;
			Vector2 f2Normal;
			return RaycastPerimeter(ray, out t, out f2HitPoint, out f2Normal, tmax);
		}
		
		public bool RaycastPerimeter(Ray ray, out float t, out Vector2 pt, out Vector2 normal, float tmax = float.PositiveInfinity)  
	    {
	        t = float.MaxValue;  
	        pt = ray.origin;  
	        normal = ray.direction;  
	          
	        // temp holder for segment distance  
	        float distance;  
			
			bool bIntersect = false;
			
			// Loop through the edges
			int iVertexCount = vertices.Count;
	        for (int j = iVertexCount - 1, i = 0; i < iVertexCount; j = i, i++)  
	        {  
	            if (RayIntersectsSegment(ray, vertices[j], vertices[i], out distance))  
	            {    
	                if (distance > 0 && distance < t && distance <= tmax)  
	                { 
						bIntersect = true;
	                    t = distance;  
	                    pt = ray.GetPoint(t);  
	
	                    Vector2 edge = vertices[j] - vertices[i];  
	                    // We would use LeftPerp() if the polygon was  
	                    // in clock wise order  
	                    normal = RightPerp(edge).normalized; 
	                }  
	            }  
	        }  
	        return bIntersect;  
	    }  
		
		public bool Raycast(Ray ray, out float t, out Vector2 pt, out Vector2 normal, float tmax = float.PositiveInfinity)  
	    {
	        t = float.MaxValue;  
	        pt = ray.origin;  
	        normal = ray.direction;  
	          
	        // temp holder for segment distance  
	        float distance;  
	        int crossings = 0;  
			
			// Loop through the edges
			int iVertexCount = vertices.Count;
	        for (int j = iVertexCount - 1, i = 0; i < iVertexCount; j = i, i++)  
	        {  
	            if (RayIntersectsSegment(ray, vertices[j], vertices[i], out distance))  
	            {  
	                crossings++;  
	                if (distance < t && distance <= tmax)  
	                {  
	                    t = distance;  
	                    pt = ray.GetPoint(t);  
	
	                    Vector2 edge = vertices[j] - vertices[i];  
	                    // We would use LeftPerp() if the polygon was  
	                    // in clock wise order  
	                    normal = RightPerp(edge).normalized;  
	                }  
	            }  
	        }  
	        return crossings > 0 && crossings % 2 == 0;  
	    }  
		
		public static bool RayIntersectsSegment(Ray ray, Vector2 pt0, Vector2 pt1, out float t, float tmax = float.PositiveInfinity) 
		{  
		    Vector2 seg = pt1 - pt0;  
		    Vector2 segPerp = RightPerp(seg);  
		    float perpDotd = Vector2.Dot(ray.direction, segPerp);  
		    if(perpDotd >= -float.Epsilon && perpDotd <= float.Epsilon)  
		    {  
		        t = float.MaxValue;  
		        return false;  
		    }  
		  
		    Vector2 d = pt0 - (Vector2)ray.origin;  
		  
		    t = Vector2.Dot(segPerp, d) / perpDotd;  
		    float s = Vector2.Dot(RightPerp(ray.direction), d) / perpDotd;  
		  
		    return t >= 0.0f && t <= tmax && s >= 0.0f && s <= 1.0f;  
		}
		
		public static Vector2 LeftPerp(Vector2 v)  
	    {  
	        return new Vector2(v.y, -v.x);  
	    }  
	
	    public static Vector2 RightPerp(Vector2 v)  
	    {  
	        return new Vector2(-v.y, v.x);  
	    }
		
		public Polygon2D()
		{
			if(vertices == null)
			{
				vertices = new List<Vector2>();
			}
		}
		
		public Polygon2D(EPolygonPrimitive a_ePrimitive)
		{
			if(vertices == null)
			{
				vertices = new List<Vector2>();
			}
			
			MakeInto(a_ePrimitive);
		}
		
		void MakeInto(EPolygonPrimitive a_ePrimitive)
		{
			switch(a_ePrimitive)
			{
				case EPolygonPrimitive.Square:
				{
					MakeIntoASquare();
				}
				break;
			}
		}
		
		void MakeIntoASquare()
		{
			vertices.Clear();
			
			AddVertex(new Vector2(-0.5f, -0.5f));
			AddVertex(new Vector2(-0.5f,  0.5f));
			AddVertex(new Vector2( 0.5f,  0.5f));
			AddVertex(new Vector2( 0.5f, -0.5f));
		}
		
		// ----- http://www.codeproject.com/Articles/15573/2D-Polygon-Collision-Detection -----
		
		// Structure that stores the results of the PolygonCollision function
		public struct PolygonCollisionResult 
		{
		    // Are the polygons going to intersect forward in time?
		    public bool WillIntersect;
		    // Are the polygons currently intersecting?
		    public bool Intersect;
		    // The translation to apply to the first polygon to push the polygons apart.
		    public Vector2 MinimumTranslationVector;
		}
		
		// Calculate the projection of a polygon on an axis
		// and returns it as a [min, max] interval
		public void ProjectPolygon(Vector2 axis, List<Vector2> a_rVertices, 
	                           ref float min, ref float max) 
		{
		    // To project a point on an axis use the dot product
		    float dotProduct = Vector2.Dot(axis, a_rVertices[0]);
		    min = dotProduct;
		    max = dotProduct;
		    for (int i = 0; i < a_rVertices.Count; i++) 
			{
		        dotProduct = Vector2.Dot(a_rVertices[i], axis);
		        if (dotProduct < min) 
				{
		            min = dotProduct;
		        }
				else 
				{
		            if (dotProduct> max) 
					{
		                max = dotProduct;
		            }
		        }
	    	}
		}
		
		// Calculate the distance between [minA, maxA] and [minB, maxB]
		// The distance will be negative if the intervals overlap
		public float IntervalDistance(float minA, float maxA, float minB, float maxB) 
		{
		    if (minA < minB) 
			{
		        return minB - maxA;
		    } 
			else 
			{
		        return minA - maxB;
		    }
		}
		
		// Check if polygon A is going to collide with polygon B.
		// The last parameter is the *relative* velocity 
		// of the polygons (i.e. velocityA - velocityB)
		public PolygonCollisionResult PolygonCollision(List<Vector2> polygonA, 
		                              List<Vector2> polygonB, Vector2 velocity) {
		    PolygonCollisionResult result = new PolygonCollisionResult();
		    result.Intersect = true;
		    result.WillIntersect = true;
		
		    int edgeCountA = polygonA.Count ;
		    int edgeCountB = polygonB.Count;
		    float minIntervalDistance = float.PositiveInfinity;
		    Vector2 translationAxis = new Vector2();
		    Vector2 edge;
		
		    // Loop through all the edges of both polygons
		    for(int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++) 
			{
		        if(edgeIndex < edgeCountA) 
				{
		            edge = polygonA[(edgeIndex+1)%edgeCountA] - polygonA[edgeIndex];
		        }
				else
				{
					int iEdgeIndexB = edgeIndex - edgeCountA;
					edge = polygonB[(iEdgeIndexB+1)%edgeCountB] - polygonB[iEdgeIndexB];
		        }
		
		        // ===== 1. Find if the polygons are currently intersecting =====
		
		        // Find the axis perpendicular to the current edge
		        Vector2 axis = new Vector2(-edge.y, edge.x);
		        axis.Normalize();
		
		        // Find the projection of the polygon on the current axis
		        float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
		        ProjectPolygon(axis, polygonA, ref minA, ref maxA);
		        ProjectPolygon(axis, polygonB, ref minB, ref maxB);
		
		        // Check if the polygon projections are currentlty intersecting
		        if (IntervalDistance(minA, maxA, minB, maxB) > 0)
				{
		            result.Intersect = false;
				}
		
		        // ===== 2. Now find if the polygons *will* intersect =====
		
		        // Project the velocity on the current axis
		        float velocityProjection = Vector2.Dot(axis, velocity);
		
		        // Get the projection of polygon A during the movement
		        if (velocityProjection < 0) {
		            minA += velocityProjection;
		        } else {
		            maxA += velocityProjection;
		        }
		
		        // Do the same test as above for the new projection
		        float intervalDistance = IntervalDistance(minA, maxA, minB, maxB);
		        if (intervalDistance > 0) result.WillIntersect = false;
		
		        // If the polygons are not intersecting and won't intersect, exit the loop
		        if (!result.Intersect && !result.WillIntersect) break;
		
		        // Check if the current interval distance is the minimum one. If so store
		        // the interval distance and the current distance.
		        // This will be used to calculate the minimum translation vector
		        intervalDistance = Math.Abs(intervalDistance);
		        if (intervalDistance < minIntervalDistance) {
		            minIntervalDistance = intervalDistance;
		            translationAxis = axis;
					
		            Vector2 d = ComputeCenter(polygonA) - ComputeCenter(polygonB);
		            if (Vector2.Dot(d, translationAxis) < 0)
		                translationAxis = -translationAxis;
		        }
		    }
		
		    // The minimum translation vector
		    // can be used to push the polygons appart.
		    if (result.WillIntersect)
		        result.MinimumTranslationVector = 
		               translationAxis * minIntervalDistance;
		    
		    return result;
		}
		
		Vector3 ComputeCenter(List<Vector2> a_rVertices)
		{
			int iVertexCount = a_rVertices.Count;
			
			if(iVertexCount == 0)
			{
				return Vector3.zero;
			}
			else
			{
				Vector2 f2Center = Vector2.zero;
				foreach(Vector2 f2Vertex in a_rVertices)
				{
					f2Center += f2Vertex;
				}
				f2Center /= iVertexCount;
				
				return f2Center;
			}
		}
	}
}