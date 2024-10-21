using UnityEngine;

namespace UniCamera
{
	[ExecuteAlways]
	[AddComponentMenu("UniCamera/AdaptCameraDistanceToEnsureWidth_Base")]
	public abstract class AdaptCameraDistanceToEnsureWidth_Base : MonoBehaviour
	{
		public float width = 10.0f;

		public bool forceUpdateInEditorPlayMode;

		public abstract float FieldOfView {get;}

		protected abstract void ApplyDistance(float distance);

		protected abstract bool Setup();

		void Start()
		{
			if(Application.isPlaying == false)
				return;

			if(Setup() == false)
				Debug.LogError("Adapt Camera is not setuped correctly", this);

			UpdateDistance();
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying && forceUpdateInEditorPlayMode == false)
				return;

			if(Setup() == false)
				return;

			UpdateDistance();
		}
		#endif

		void UpdateDistance()
		{
			ApplyDistance(ComputeDistance());
		}

		float ComputeDistance()
		{
			float distanceToFitWidth = CameraUtility.ComputeCameraDistance_ByWidth(width, FieldOfView);

			return distanceToFitWidth;
		}
	}
}