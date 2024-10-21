using UnityEngine;
using System.Collections;

using UniButton;

namespace UniCamera
{
	[AddComponentMenu("UniCamera/OrbitalCamera_Orbit")]
	public class OrbitalCamera_Orbit : MonoBehaviour 
	{
		public OrbitalCamera orbitalCamera;

		public float inputRotationSpeed = 1080.0f;

		public float smoothTime = 0.2f;

		public bool inputEnabled = true;

		public TouchButtonController touchButtonController;

		Vector3 currentOrientation;
		Vector3 targetOrientation;
		Vector3 currentVelocity;

		public Vector3 InitialOrientation {get; private set;}

		public Vector3 TargetOrientation
		{
			get => targetOrientation;

			set
			{
				targetOrientation = value;
			}
		}

		void Start()
		{
			currentOrientation = transform.eulerAngles;

			InitialOrientation = currentOrientation;

			targetOrientation = currentOrientation;
		}

		void Update()
		{
			if(inputEnabled && touchButtonController.controlledButton.Pressed)
			{
				Vector2 rotation;

				rotation.x = -Input.GetAxis("Mouse X") * inputRotationSpeed * Time.deltaTime;
				rotation.y = -Input.GetAxis("Mouse Y") * inputRotationSpeed * Time.deltaTime;

				targetOrientation.x += rotation.y;
				targetOrientation.y += -rotation.x;
			}

			currentOrientation =
				Vector3.SmoothDamp(currentOrientation, targetOrientation, ref currentVelocity, smoothTime);

			orbitalCamera.Rotation = Quaternion.Euler(currentOrientation);
		}
	}
}