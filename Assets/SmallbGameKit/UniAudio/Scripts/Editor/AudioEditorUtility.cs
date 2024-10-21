using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEditor;

namespace UniAudio
{
	public class AudioEditorUtility
	{
		public static Editor_PreviewSoundController PlaySound(SoundAsset soundAsset) 
		{
			return PlaySound(soundAsset.Sound.NextPlaySoundCommand);
		}

		public static Editor_PreviewSoundController PlaySound(PlaySoundCommand soundCommand) 
		{
			return PlayClip(soundCommand.audioClip, soundCommand.Volume);
		}

		public static Editor_PreviewSoundController PlayClip(AudioClip clip, float volume = 1.0f) 
		{
			GameObject audioSourceGameObject = new GameObject("PreviewTempAudioSource");
			AudioSource audioSource = audioSourceGameObject.AddComponent<AudioSource>();
			audioSource.PlayOneShot(clip, volume);

			audioSourceGameObject.hideFlags = HideFlags.HideAndDontSave;

			return Editor_PreviewSoundController.Create(audioSource);
		}
	}
}