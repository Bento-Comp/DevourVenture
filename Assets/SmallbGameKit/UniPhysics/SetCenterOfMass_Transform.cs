using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPhysics
{
	[AddComponentMenu("UniPhysics/SetCenterOfMass_Transform")]
	public class SetCenterOfMass_Transform : MonoBehaviour 
	{
		public Transform centerOfMassTransform;

		public Rigidbody rigidbodyComponent;

		public bool debug_UpdateInEditorRuntime;

        void Awake()
        {
            SetCenterOfMass();
        }

#if UNITY_EDITOR
		void FixedUpdate()
		{
			if(debug_UpdateInEditorRuntime == false)
				return;

			SetCenterOfMass();
		}
#endif

		void SetCenterOfMass()
		{
			rigidbodyComponent.centerOfMass = rigidbodyComponent.transform.InverseTransformPoint(centerOfMassTransform.position);
		}
	}
}
