using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPhysics
{
	[SelectionBase]
	[AddComponentMenu("UniPhysics/StartAsleep")]
	public class StartAsleep : MonoBehaviour
	{
		public Rigidbody rigidbodyComponent;

		void OnEnable()
		{
			rigidbodyComponent.Sleep();
		}
	}
}