using UnityEngine;

namespace UniCamera
{
	public static class CameraUtility
	{
		public static float ComputeCameraDistance_ByWidth(float viewWidth, float fovHeight)
		{
			return ComputeCameraDistance_ByHeight(viewWidth, FovHeightToFovWidth(fovHeight));
		}

		public static float ComputeCameraDistance_ByHeight(float viewHeight, float fovHeight)
		{
			float alpha = fovHeight/2.0f;
			float l = viewHeight/2.0f;

			return l / Mathf.Tan(alpha * Mathf.Deg2Rad);
		}

		public static float FovHeightToFovWidth(float fovHeight)
		{
			float ratio = ((float)Screen.width) / ((float)Screen.height);
			float fovWidht = 2 * Mathf.Atan(ratio * Mathf.Tan((fovHeight * Mathf.Deg2Rad) / 2));
			return fovWidht * Mathf.Rad2Deg;
		}
	}
}