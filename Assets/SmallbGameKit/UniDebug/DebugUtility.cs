using UnityEngine;

namespace UniDebug
{
	public static class DebugUtility
	{
		public static void DrawPoint(Vector3 position)
		{
			DrawPoint(position, Color.cyan);
		}

		public static void DrawPoint(Vector3 position, Color color, float duration = 1.0f, float radius = 0.025f)
		{
			Vector3 up = position + Vector3.up * radius;
			Vector3 down = position + Vector3.down * radius;
			Vector3 forward = position + Vector3.forward * radius;
			Vector3 back = position + Vector3.back * radius;
			Vector3 left = position + Vector3.left * radius;
			Vector3 right = position + Vector3.right * radius;

			Debug.DrawLine(up, down, color, duration);
			Debug.DrawLine(forward, back, color, duration);
			Debug.DrawLine(left, right, color, duration);
		}
	}
}
