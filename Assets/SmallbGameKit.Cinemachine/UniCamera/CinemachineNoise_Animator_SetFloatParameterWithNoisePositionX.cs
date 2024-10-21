using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Cinemachine;

namespace UniCamera
{
	[AddComponentMenu("UniCamera/CinemachineNoise_Animator_SetFloatParameterWithNoisePositionX")]
	public class CinemachineNoise_Animator_SetFloatParameterWithNoisePositionX : MonoBehaviour
	{
		public Animator animator;

		public NoiseSettings noiseSettings;

		public string floatName = "Noise";

		public float remapping_NoiseAmplitudeMax = 1.0f;

		void Update()
		{
			float noise = NoiseSettings.GetCombinedFilterResults(noiseSettings.PositionNoise, Time.time, Vector3.zero).x;

			float maxExtent = remapping_NoiseAmplitudeMax * 0.5f;
			float remappedNoise = Mathf.InverseLerp(-maxExtent, maxExtent, noise);

			animator.SetFloat(floatName, remappedNoise);
		}
	}
}