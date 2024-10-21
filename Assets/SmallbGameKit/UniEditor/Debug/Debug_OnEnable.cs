using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniEditor
{
	[AddComponentMenu("UniEditor/Debug_OnEnable")]
	public class Debug_OnEnable : MonoBehaviour 
	{
		void OnEnable()
		{
			Debug.Log("On Enable : " + this);
		}
	}
}