using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace UniApplication
{
	public static class Quality
	{	
		#if UNITY_ANDROID
		static bool lowEndOverride;
		#endif

		public static bool HighEnd
		{
			get
			{
				return !LowEnd;
			}

			set
			{
				LowEnd = !value;
			}
		}

		public static bool LowEnd
		{
			get
			{
				#if UNITY_IOS
				if(UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone
				   ||
				   UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone3G
				   ||
				   UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone3GS
				   ||
				   UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone4
				   ||
				   UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPodTouch1Gen
				   ||
				   UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPodTouch2Gen
				   ||
				   UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPodTouch3Gen
				   ||
				   UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPodTouch4Gen
				   ||
				   UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPodTouch5Gen
				   ||
				   UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad1Gen
				   )
				{
					return true;
				}
				#elif UNITY_ANDROID
				if(lowEndOverride)
				{
					return true; 
				}
				#endif

				return false;
			}

			set
			{
				#if UNITY_ANDROID
				//Debug.Log(value);
				lowEndOverride = value;
				#endif
			}
		}
	}
}