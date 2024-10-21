using UnityEngine;

namespace UniFillBar
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniFillBar/RadialFillBarLayout")]
	public class RadialFillBarLayout : FillBarLayoutBase
	{
		public Transform containerPivots;

		public float beginAngle = -30.0f;
		public float angleBetweenPart = -30.0f;

		public override void UpdateLayout()
		{
			float rotationZ = beginAngle;

			Transform pivot = containerPivots;
			if(pivot == null)
				pivot = transform;

			foreach(Transform children in pivot)
			{
				children.localRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
				rotationZ += angleBetweenPart;
			}
		}
	}
}