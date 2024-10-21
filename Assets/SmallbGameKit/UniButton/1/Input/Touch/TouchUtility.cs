using UnityEngine;
using System.Collections;

namespace UniButton
{
	public static class TouchUtility 
	{
		static bool deactivateMultiTouch = false;

		static float defaultDPI = 326.0f;

		static float inchToCentimeters = 2.54f;
		
		public static bool MultiTouchEnabled
		{
			get
			{
				return Input.multiTouchEnabled && (deactivateMultiTouch == false);
			}
		}

		public static float DPI
		{
			get
			{
				float dpi = Screen.dpi;
				if(dpi <= 0)
				{
					dpi = defaultDPI;
				}
				#if UNITY_EDITOR
				if(TouchUtilityManager.Instance != null && TouchUtilityManager.Instance.editor_override_dpi)
					dpi = TouchUtilityManager.Instance.editor_dpi_override;
				#endif

				/*if(Application.isPlaying)
					Debug.Log("dpi = " + dpi);*/

				return dpi;
			}
		}

		public static float PixelToCentimeter
		{
			get
			{
				return inchToCentimeters/DPI;
			}
		}

		public static float CentimetersToPixel
		{
			get
			{
				return DPI/inchToCentimeters;
			}
		}

		public static bool TryGetTouchByFingerId(int a_iFingerId, out Touch a_rTouch)
		{
			a_rTouch = default(Touch);

			// loop through the touches
			foreach(Touch rTouch in Input.touches)
			{
				if(rTouch.fingerId == a_iFingerId)
				{
					a_rTouch = rTouch;
					return true;
				}
			}

			return false;
		}
	}
}