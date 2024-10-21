using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniAudio
{
	[AddComponentMenu("UniAudio/AudioSourceController")]
	public class AudioSourceController : MonoBehaviour
	{
		[System.Serializable]
		public class NotAnimatable
		{
			public float baseVolume;

			public float basePitch;

			public NotAnimatable()
			{
				baseVolume = 1.0f;
				basePitch = 1.0f;
			}
		}

		[SerializeField]
		NotAnimatable notAnimatable = new NotAnimatable();

		[SerializeField]
		float animatedVolumePercent = 1.0f;

		[SerializeField]
		float animatedPitchPercent = 1.0f;

		float volumeScale = 1.0f;

		float pitchScale = 1.0f;

		AudioSource audioSource;

		public bool Valid
		{
			get
			{
				if(isActiveAndEnabled == false)
					return false;

				return true;
			}
		}

		public AudioSource AudioSource
		{
			get
			{
				return audioSource;
			}
		}

		public float VolumeScale
		{
			get
			{
				return volumeScale;
			}

			set
			{
				volumeScale = value;
			}
		}

		public float PitchScale
		{
			get
			{
				return pitchScale;
			}

			set
			{
				pitchScale = value;
			}
		}

		void Awake()
		{
			audioSource = GetComponent<AudioSource>(); 
			AudioManager.Instance.Register(this);
		}

		void OnDestroy()
		{
			if(AudioManager.Instance != null)
				AudioManager.Instance.Unregister(this);
		}

		void LateUpdate()
		{
			audioSource.mute = AudioManager.Instance.Mute;
			audioSource.volume = animatedVolumePercent * notAnimatable.baseVolume * volumeScale;
			audioSource.pitch = animatedPitchPercent * notAnimatable.basePitch * pitchScale;
		}
	}
}