using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniCurves
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniCurves/BezierCurve_2DCollider")]
	public class BezierCurve_2DCollider : MonoBehaviour
	{
		BezierCurve bezierCurve;

		EdgeCollider2D edgeCollider;

		void Awake()
		{
			GetComponents();

			UpdateCollider();
		}

		void Update()
		{
			if(Application.isPlaying)
				return;

			GetComponents();

			if(bezierCurve == null || edgeCollider == null)
				return;

			UpdateCollider();
		}

		void UpdateCollider()
		{
			List<Vector3> vertices = bezierCurve.ComputeVertices();

			Vector2[] vertices2D = new Vector2[vertices.Count];

			for(int i = 0; i < vertices.Count; ++i)
			{
				vertices2D[i] = edgeCollider.transform.InverseTransformPoint(vertices[i]);
			}

			edgeCollider.points = vertices2D;
		}

		void GetComponents()
		{
			bezierCurve = GetComponent<BezierCurve>();
			edgeCollider = GetComponent<EdgeCollider2D>();
		}
	}
}