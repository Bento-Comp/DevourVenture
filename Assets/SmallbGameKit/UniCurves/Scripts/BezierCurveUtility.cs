using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniCurves
{
	public static class BezierCurveUtility
	{	
		public static List<Vector3> ComputeVertices(List<BezierControlPoint> points, int verticesBySegment)
		{
			List<Vector3> vertices = new List<Vector3>();
			for(int p = 0; p < points.Count - 1; ++p)
			{
				BezierControlPoint begin = points[p];
				BezierControlPoint end = points[p+1];

				for(int i = 0; i < verticesBySegment; ++i)
				{
					float percent = i/(float)(verticesBySegment - 1);
					Vector3 vertex = BezierCurveUtility.EvaluateBezier(percent, begin.position, begin.position + begin.rightTangent,
						end.position + end.leftTangent, end.position);
					vertices.Add(vertex);
				}
			}

			return vertices;
		}

		public static List<Vector3> ComputeTangents(List<BezierControlPoint> points, int verticesBySegment)
		{
			List<Vector3> vertices = new List<Vector3>();
			for(int p = 0; p < points.Count - 1; ++p)
			{
				BezierControlPoint begin = points[p];
				BezierControlPoint end = points[p+1];

				for(int i = 0; i < verticesBySegment; ++i)
				{
					float percent = i/(float)(verticesBySegment - 1);
					Vector3 vertex = BezierCurveUtility.EvaluateBezierTangent(percent, begin.position, begin.position + begin.rightTangent,
						end.position + end.leftTangent, end.position);
					vertices.Add(vertex);
				}
			}

			return vertices;
		}

		public static Vector3 EvaluateBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			float u = 1.0f - t;
			float tt = t*t;
			float uu = u*u;
			float uuu = uu * u;
			float ttt = tt * t;
		 
			float a0 = uuu;
			float a1 = 3 * uu * t;
			float a2 = 3 * u * tt;
			float a3 = ttt;

		 	Vector3 p = a0 * p0; //first term
		 	p += a1 * p1; //second term
		 	p += a2 * p2; //third term
		 	p += a3 * p3; //fourth term
		 
			return p;
		}

		public static Vector3 EvaluateBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			float u = 1.0f - t;
			float tt = t*t;
			float uu = u*u;
			float ut = u*t;

			float a0 = -3 * uu;
			float a1 = 3 * uu - 6 * ut;
			float a2 = -3 * tt + 6 * ut;
			float a3 = 3 * tt;

			Vector3 p = a0 * p0; //first term
			p += a1 * p1; //second term
			p += a2 * p2; //third term
			p += a3 * p3; //fourth term

			return p;
		}
	}
}