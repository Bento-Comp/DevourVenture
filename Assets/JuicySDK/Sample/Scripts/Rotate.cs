using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Rotate")]
	public class Rotate : MonoBehaviour
	{
		public float rotateSpeed = 90.0f;

		public Vector3 rotateAxis = new Vector3(0.5f, 1.0f, 0.25f);
	
		void Update()
		{
			transform.Rotate(rotateAxis, rotateSpeed * Time.deltaTime);
		}
	}
}
