using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniCurves
{
	[System.Serializable]
	public class BezierControlPoint
	{
		public Vector3 position;

		public Vector3 leftTangent;

		public Vector3 rightTangent;
	}
}