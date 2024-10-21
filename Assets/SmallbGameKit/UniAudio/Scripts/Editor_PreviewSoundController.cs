using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniAudio
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniAudio/Editor_PreviewSoundController")]
	public class Editor_PreviewSoundController : MonoBehaviour
	{
		public AudioSource watchedAudioSource;

		public static Editor_PreviewSoundController Create(AudioSource watchedAudioSource)
		{
			Editor_PreviewSoundController controller = watchedAudioSource.gameObject.AddComponent<Editor_PreviewSoundController>();
			controller.watchedAudioSource = watchedAudioSource;

			return controller;
		}

		void LateUpdate()
		{
			if(watchedAudioSource != null)
			{
				if(watchedAudioSource.isPlaying == false)
				{
					DestroyImmediate(gameObject);
				}
			}
		}
	}
}