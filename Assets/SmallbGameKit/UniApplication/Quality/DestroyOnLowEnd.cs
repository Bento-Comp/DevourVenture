using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace UniApplication
{
	[AddComponentMenu("UniApplication/Quality/DestroyOnLowEnd")]
	public class DestroyOnLowEnd : MonoBehaviour
	{	
		void Awake()
		{
			if(Quality.LowEnd)
			{
				Destroy(gameObject);
			}
		}
	}
}