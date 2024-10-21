using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniCurves
{
	[AddComponentMenu("UniCurves/BezierPoint")]
	public class BezierPoint : MonoBehaviour
	{
		[System.Serializable]
		public class GizmoParameters
		{
			public Color pointColor = Color.cyan;
		}

		public BezierTangentPoint leftTangent;

		public BezierTangentPoint rightTangent;

		public GizmoParameters gizmo;

		void OnDrawGizmos()
		{
			Gizmos.color = gizmo.pointColor;
			
			float pointSize = 0.1f;

			Gizmos.DrawWireSphere(transform.position, pointSize);

			if(leftTangent != null)
			{
				Gizmos.DrawLine(transform.position, leftTangent.transform.position);
			}

			if(rightTangent != null)
			{
				Gizmos.DrawLine(transform.position, rightTangent.transform.position);
			}
		}
	}
}