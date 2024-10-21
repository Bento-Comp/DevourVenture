using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

namespace UniCamera
{
	[AddComponentMenu("UniCamera/OrbitalCamera")]
	public class OrbitalCamera : MonoBehaviour
	{
		public CinemachineVirtualCamera virtualCamera;

		public Quaternion Rotation
		{
			get => virtualCamera.transform.rotation;
			set => virtualCamera.transform.rotation = value;
		}
	}
}
