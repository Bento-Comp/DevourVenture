using UnityEngine;

namespace UniCamera
{
	[ExecuteAlways]
	[DefaultExecutionOrder(1000)]
	[AddComponentMenu("UniCamera/CameraPropertySlave")]
	public class CameraPropertySlave : MonoBehaviour
	{
		public Camera masterCamera;

		public Camera slaveCamera;

		public bool updateAtStart = false;

		public bool updateOnEnable = true;

		public bool updateInPlayMode = true;

		public bool updateInEditMode = true;

		void OnEnable()
		{
#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
#endif

			if(updateOnEnable == false)
				return;

			UpdateSlaveCamera();
		}

		void Start()
		{
#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
#endif

			if(updateAtStart == false)
				return;

			UpdateSlaveCamera();
		}

		void LateUpdate()
		{
#if UNITY_EDITOR
			if(updateInEditMode == false && Application.isPlaying == false)
				return;
#endif
			if(updateInPlayMode == false && Application.isPlaying)
				return;

			UpdateSlaveCamera();
		}

		void UpdateSlaveCamera()
		{
#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				if(slaveCamera == null)
					return;

				if(masterCamera == null)
					return;
			}
#endif
			slaveCamera.fieldOfView = masterCamera.fieldOfView;
			//slaveCamera.nearClipPlane = masterCamera.nearClipPlane;
			//slaveCamera.farClipPlane = masterCamera.farClipPlane;
		}
	}
}