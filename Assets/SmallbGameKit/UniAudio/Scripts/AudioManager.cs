using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniAudio
{
	[AddComponentMenu("UniAudio/AudioManager")]
	public class AudioManager : MonoBehaviour
	{
		public List<AudioSourceController> defaultSoundAudioSources;

		public List<SoundAsset> soundAssetsCallableByName;

		Dictionary<string, Sound> soundByName = new Dictionary<string, Sound>();

		Dictionary<string, AudioSourceController> audioSourceByNames = new Dictionary<string, AudioSourceController>();

		static AudioManager instance;

		bool mute;
		
		public static AudioManager Instance
		{
			get
			{
				return instance;
			}
		}

		public bool Mute
		{
			get
			{
				return mute;
			}

			set
			{
				mute = value;
				LastLaunchMute = value;
			}
		}

		public AudioSourceController MainActiveAudioSourceController
		{
			get
			{
				foreach(AudioSourceController audioSourceController in defaultSoundAudioSources)
				{
					if(audioSourceController.isActiveAndEnabled == false)
					{
						continue;
					}

					return audioSourceController;
				}

				Debug.LogWarning("No Audio Source Controller Available!");
				return null;
			}
		}

		static string lastLaunchMuteSaveKey = "Mute";
		bool LastLaunchMute
		{
			get
			{
				return PlayerPrefs.GetInt(lastLaunchMuteSaveKey, 0) != 0;
			}

			set
			{
				PlayerPrefs.SetInt(lastLaunchMuteSaveKey, value?1:0);
			}
		}

		public void Register(AudioSourceController audioSourceController)
		{
			audioSourceByNames.Add(audioSourceController.name, audioSourceController);	
		}

		public void Unregister(AudioSourceController audioSourceController)
		{
			audioSourceByNames.Remove(audioSourceController.name);	
		}

		public void PlaySound(SoundProperty soundProperty, float volume = 1.0f)
		{
			switch(soundProperty.type)
			{
				case SoundProperty.EPropertyType.ByName:
				{
					AudioManager.Instance.PlaySound(soundProperty.soundName, volume);
				}
				break;

				default:
				case SoundProperty.EPropertyType.Asset:
				{
					AudioManager.Instance.PlaySound(soundProperty.soundAsset ,volume);
				}
				break;
			}
		}

		public void PlaySound(string soundName, float volume = 1.0f)
		{
			Sound sound;
			if(soundByName.TryGetValue(soundName, out sound))
			{
				//Debug.Log("Play Sound : " + soundName);
				PlaySound(sound, volume);
			}
			else
			{
				Debug.LogWarning("Sound not found : " + soundName);
			}
		}

		public void PlaySound(SoundAsset soundAsset, float volume = 1.0f)
		{
			if(soundAsset == null)
			{
				Debug.LogWarning("SoundAsset null");
				return;
			}

			//Debug.Log("Play Sound : " + soundAsset);

			PlaySound(soundAsset.Sound, volume);
		}

		public void PlaySound(Sound sound, float volume = 1.0f)
		{
			PlaySound(sound.NextPlaySoundCommand, volume);
		}

		public void PlaySound(PlaySoundCommand playSoundCommand, float volume = 1.0f)
		{
			// Try to get an audio source controller
			AudioSourceController audioSourceController = null;
			float effectiveVolume = playSoundCommand.Volume * volume;
			if(playSoundCommand.audioSourceName != "")
			{
				if(audioSourceByNames.TryGetValue(playSoundCommand.audioSourceName, out audioSourceController) == false)
				{
					Debug.LogWarning("Audio source controller : " + playSoundCommand.audioSourceName + " not found.");
				}
			}

			if(playSoundCommand.setAndPlay)
			{
				// Set and play
				SetAndPlaySound(audioSourceController, playSoundCommand.audioClip, effectiveVolume, playSoundCommand.loop, playSoundCommand.pitch);
			}
			else
			{
				// Play one shot
				PlaySound(playSoundCommand.audioClip, audioSourceController, effectiveVolume);
			}
		}

		public void SetAndPlaySound(AudioSourceController audioSourceController, AudioClip clip, float volumeScale, bool loop = false, float pitch = 1.0f)
		{
			if(audioSourceController == null)
			{
				audioSourceController = MainActiveAudioSourceController;
				if(audioSourceController == null)
					return;
			}
			
			audioSourceController.AudioSource.clip = clip;
			audioSourceController.AudioSource.loop = loop;
			audioSourceController.PitchScale = pitch;

			audioSourceController.VolumeScale = volumeScale;

			if(audioSourceController.AudioSource.isActiveAndEnabled)
			{
				audioSourceController.AudioSource.Play();
			}
			else
			{
				audioSourceController.AudioSource.playOnAwake = true;
			}
		}

		public void PlaySound(AudioClip clip, float volumeScale = 1.0f)
		{
			if(mute)
				return;

			AudioSourceController audioSourceController = MainActiveAudioSourceController;
			if(audioSourceController != null)
			{
				audioSourceController.AudioSource.PlayOneShot(clip, volumeScale);
			}
		}

		public void PlaySound(AudioClip clip, AudioSource audioSource)
		{
			if(mute)
				return;

			audioSource.PlayOneShot(clip);
		}

		public void PlaySound(AudioClip clip, AudioSourceController audioSourceController, float volumeScale)
		{
			if(mute)
				return;
			
			if(audioSourceController == null)
				audioSourceController = MainActiveAudioSourceController;

			if(audioSourceController == null)
				return;

			PlaySound(clip, audioSourceController.AudioSource, volumeScale);
		}

		public void PlaySound(AudioClip clip, AudioSource audioSource, float volumeScale)
		{
			if(mute)
				return;

			if(audioSource == null)
			{
				PlaySound(clip, volumeScale);
			}
			else
			{
				if(audioSource.isActiveAndEnabled == false)
					return;
				
				audioSource.PlayOneShot(clip, volumeScale);
			}
		}

		void Awake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}

			foreach(SoundAsset soundAsset in soundAssetsCallableByName)
			{
				soundByName.Add(soundAsset.name, soundAsset.Sound);
			}

			Mute = LastLaunchMute;
		}
		
		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}
	}
}