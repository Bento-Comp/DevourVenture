using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cinemachine;

namespace UniCamera
{
	[ExecuteAlways()]
	[AddComponentMenu("UniCamera/AdaptVirtualCameraDistanceToEnsureWidth")]
	public class AdaptVirtualCameraDistanceToEnsureWidth : AdaptCameraDistanceToEnsureWidth_Base
	{
		public CinemachineVirtualCamera virtualCamera;
	
		CinemachineFramingTransposer framingTransposer;

		public override float FieldOfView => virtualCamera.m_Lens.FieldOfView;

		protected override bool Setup()
		{
			framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

			return framingTransposer != null;
		}

		protected override void ApplyDistance(float distance)
		{
			framingTransposer.m_CameraDistance = distance;
		}
	}
}