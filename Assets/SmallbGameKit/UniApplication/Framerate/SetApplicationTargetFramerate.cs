using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniApplication
{
	[AddComponentMenu("UniApplication/SetApplicationTargetFramerate")]
	public class SetApplicationTargetFramerate : MonoBehaviour
	{
		public int highEndTargetFramerate = 60;

		public int lowEndTargetFramerate = 60;

		void Awake()
		{
			if(Quality.LowEnd)
			{
				Application.targetFrameRate = lowEndTargetFramerate;
			}
			else
			{
				Application.targetFrameRate = highEndTargetFramerate;
			}
		}
	}
}