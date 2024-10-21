using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniCurves
{
	[AddComponentMenu("UniCurves/BezierTangentPoint")]
	public class BezierTangentPoint : MonoBehaviour
	{
		void OnDrawGizmos()
		{
			float tangentHandleSize = 0.1f;

			Vector3 tangentHandleSizeVector = Vector3.one * tangentHandleSize;

			Gizmos.DrawWireCube(transform.position, tangentHandleSizeVector);
		}
	}
}