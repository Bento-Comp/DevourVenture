using UnityEngine;

namespace UniUtilities
{
	public static class Angle3DUtility
	{
		public static float SignedAngleAroundAxis(Vector3 from, Vector3 to, Vector3 axis)
        {
			from.Normalize();
			to.Normalize();

            Vector3 right = Vector3.Cross(axis, from);
            from = Vector3.Cross(right, axis);
        
            return Mathf.Atan2(Vector3.Dot(to, right), Vector3.Dot(to, from)) * Mathf.Rad2Deg;
        }
	}
}
