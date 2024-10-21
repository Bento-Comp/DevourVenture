using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniCamera
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniCamera/OrbitalCamera_Zoom")]
	public class OrbitalCamera_Zoom : MonoBehaviour
	{
		public OrbitalCamera orbitalCamera;

		public float fieldOfViewZoomMin = 40.0f;
		public float fieldOfViewZoomMax = 10.0f;

		public float inputZoomSpeed = 0.5f;

		public float smoothTime = 0.2f;

		public bool inputEnabled = true;

		[Range(0.0f, 1.0f)]
		[SerializeField]
		float zoomPercent;

		float currentVelocity;

		float zoomPercentTarget;

		public float InitialZoomPercent {get; private set;}

		public float TargetZoomPercent
		{
			get => zoomPercentTarget;
			set => zoomPercentTarget = Mathf.Clamp01(value);
		}

		public float FieldOfViewScale
		{
			get => GetFieldOfView(zoomPercent)/GetFieldOfView(0.0f);
		}

		float ZoomPercent
		{
			get => zoomPercent;
			set
			{
				zoomPercent = value;

				zoomPercent = Mathf.Clamp01(zoomPercent);

				ApplyZoomPercent();
			}
		}

		void Start()
		{
			zoomPercentTarget = ZoomPercent;
			InitialZoomPercent = ZoomPercent;
		}

		void Update()
		{
			UpdateZoom();
		}

		void UpdateZoom()
		{

#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				if(orbitalCamera == null)
					return;

				if(orbitalCamera.virtualCamera == null)
					return;
			}
			else
#endif
			{
				UpdateZoomPercent();
			}

			ApplyZoomPercent();
		}

		void ApplyZoomPercent()
		{
			orbitalCamera.virtualCamera.m_Lens.FieldOfView = GetFieldOfView(zoomPercent);
		}

		void UpdateZoomPercent()
		{
			if(inputEnabled)
			{
				TargetZoomPercent += Input.mouseScrollDelta.y * inputZoomSpeed;
			}

			ZoomPercent = Mathf.SmoothDamp(ZoomPercent,
				TargetZoomPercent,
				ref currentVelocity,
				smoothTime);
		}

		float GetFieldOfView(float zoomPercent)
		{
			return Mathf.Lerp(fieldOfViewZoomMin, fieldOfViewZoomMax, zoomPercent);
		}
	}
}
