using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UniSpikeFixer
{
	[AddComponentMenu("UniSpikeFixer/SpikeFixerManager")]
	public class SpikeFixerManager : MonoBehaviour 
	{
		public float addProcessingTimeMilliseconds = 8.0f;

		DateTime then;

		void LateUpdate()
		{
			float timeToAdd = addProcessingTimeMilliseconds - Time.deltaTime;
			then = DateTime.Now;
			while(true)
			{
				DateTime now = DateTime.Now;	

				if((now - then).Milliseconds > (timeToAdd - 1.0f))
					break;
			}
		}
	}
}
