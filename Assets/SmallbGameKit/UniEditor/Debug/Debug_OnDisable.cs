using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniEditor
{
	[AddComponentMenu("UniEditor/Debug_OnEnable")]
	public class Debug_OnDisable : MonoBehaviour 
	{
		void OnDisable()
		{
			Debug.Log("On Disable : " + this);
		}
	}
}