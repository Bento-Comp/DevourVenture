using UnityEngine;

namespace UniUI
{
	[ExecuteInEditMode()]
	[DefaultExecutionOrder(100)]
	[AddComponentMenu("UniUI/LookAtCamera")]
	public class LookAtCamera : MonoBehaviour
	{
		void LateUpdate()
		{
			UpdateLookAt();
		}

		void UpdateLookAt()
		{
			Vector3 billboardDirection = -Camera.main.transform.forward;
			transform.forward = billboardDirection;
		}
	}
}