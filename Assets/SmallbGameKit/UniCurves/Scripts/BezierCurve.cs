using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniCurves
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniCurves/BezierCurve")]
	public class BezierCurve : MonoBehaviour
	{
		[System.Serializable]
		public class GizmoParameters
		{
			public bool displayTangents;

			public float tangentScale = 1.0f;

			public Color curveColor = Color.white;

			public Color tangentColor = Color.cyan;
		}

		public List<BezierPoint> points = new List<BezierPoint>();

		public bool autoFill = true;

		public bool autoName = false;

		public int verticesBySegment = 10;

		public GizmoParameters gizmo = new GizmoParameters();

		public float GetCurveLengthApproximation(int vertexCount)
		{
			float curveLength = 0.0f;
			Vector3 lastPathPoint = Vector3.zero;
			for(int i = 0; i < vertexCount; ++i)
			{
				float curvePercent = (float)i/(float)(vertexCount - 1);

				Vector3 pathPoint = GetPoint(curvePercent);

				if(i > 0)
				{
					curveLength += (lastPathPoint - pathPoint).magnitude;
				}
				lastPathPoint = pathPoint;
			}

			return curveLength;
		}

		public List<Vector3> ComputeVertices()
		{
			return BezierCurveUtility.ComputeVertices(ComputeControlPoints(), verticesBySegment);
		}

		public List<Vector3> ComputeTangents()
		{
			return BezierCurveUtility.ComputeTangents(ComputeControlPoints(), verticesBySegment);
		}

		public Vector3 GetPointAlongTheCurve(float beginPercent, float distanceAlongTheCurve, out float endPercent, float stepInPercent = 0.01f, float treshold = 0.1f)
		{
			Vector3 beginPoint = GetPoint(beginPercent);
			Vector3 currentPoint = beginPoint;
			float currentDistanceAlongTheCurve = 0.0f;
			float currentPercent = beginPercent;
			// Todo_Sev: float previousPercent = beginPercent;

			// Travel along the curve step by step until we pass the wanted distance
			while(currentDistanceAlongTheCurve < distanceAlongTheCurve)
			{
				// Todo_Sev: previousPercent = currentPercent;
				currentPercent += stepInPercent;

				if(currentPercent >= 1.0f)
				{
					endPercent = 1.0f;
					return GetPoint(endPercent);
				}

				Vector3 nextPoint = GetPoint(currentPercent);
		
				currentDistanceAlongTheCurve += (nextPoint - currentPoint).magnitude;

				currentPoint = nextPoint;
			}

			// Todo_Sev: Now we know that the wanted point is in the last segment
			// Use a dichotomy to narrow is position down


			endPercent = currentPercent;
			return currentPoint;
		}

		public Vector3 GetPoint(float percent)
		{
			if(points.Count <= 0)
				return transform.position;
			
			if(percent >= 1.0f)
				return points[points.Count - 1].transform.position;
					
			int beginPointIndex = Mathf.FloorToInt(percent * (points.Count - 1));

			BezierPoint begin = points[beginPointIndex];
			BezierPoint end = points[beginPointIndex+1];

			float beginSegmentInCurvePercent = (float)beginPointIndex/(float)(points.Count-1);
			float segmentPercent = (percent - beginSegmentInCurvePercent) * (float)(points.Count-1);

			Vector3 point = BezierCurveUtility.EvaluateBezier(segmentPercent, begin.transform.position, begin.rightTangent.transform.position,
				end.leftTangent.transform.position, end.transform.position);

			return point;
		}

		public Vector3 GetTangent(float percent)
		{
			if(points.Count <= 0)
				return transform.position;

			int beginPointIndex = Mathf.Clamp(Mathf.FloorToInt(percent * (points.Count - 1)), 0, points.Count - 2);

			BezierPoint begin = points[beginPointIndex];
			BezierPoint end = points[beginPointIndex+1];

			float beginSegmentInCurvePercent = (float)beginPointIndex/(float)(points.Count-1);
			float segmentPercent = (percent - beginSegmentInCurvePercent) * (float)(points.Count-1);

			Vector3 point = BezierCurveUtility.EvaluateBezierTangent(segmentPercent, begin.transform.position, begin.rightTangent.transform.position,
				end.leftTangent.transform.position, end.transform.position);

			return point;
		}

		void Update()
		{
			if(Application.isPlaying)
				return;

			if(autoFill)
			{
				points.Clear();
				points.AddRange(GetComponentsInChildren<BezierPoint>());

				if(autoName)
				{
					int index = 0;
					foreach(BezierPoint point in GetComponentsInChildren<BezierPoint>())
					{
						++index;
						point.name = index.ToString();
					}
				}
			}
		}

		void OnDrawGizmos()
		{
			Gizmos.color = gizmo.curveColor;

			List<Vector3> vertices = ComputeVertices();
			for(int i = 0; i < vertices.Count - 1; ++i)
			{
				Gizmos.DrawLine(vertices[i], vertices[i+1]); 
			}

			if(gizmo.displayTangents)
			{
				Gizmos.color = gizmo.tangentColor;
				List<Vector3> tangents = ComputeTangents();
				for(int i = 0; i < vertices.Count - 1; ++i)
				{
					Vector3 tangent = tangents[i] * gizmo.tangentScale;
					Vector3 vertex = vertices[i];
					Gizmos.DrawLine(vertex - tangent * 0.5f, vertex + tangent * 0.5f); 
				}
			}
		}

		List<BezierControlPoint> ComputeControlPoints()
		{
			List<BezierControlPoint> controlPoints = new List<BezierControlPoint>();

			foreach(BezierPoint point in points)
			{
				BezierControlPoint controlPoint = new BezierControlPoint();

				if(point != null)
				{
					controlPoint.position = point.transform.position;
					if(point.leftTangent != null)
						controlPoint.leftTangent = point.leftTangent.transform.position - point.transform.position;

					if(point.rightTangent != null)
						controlPoint.rightTangent = point.rightTangent.transform.position - point.transform.position;
				}
				controlPoints.Add(controlPoint);
			}

			return controlPoints;
		}
	}
}