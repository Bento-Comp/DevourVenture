using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace UniAudio
{
	[AddComponentMenu("UniAudio/PlaySoundOnClick")]
	public class PlaySoundOnClick : MonoBehaviour 
	{
		public SoundProperty sound;

		Button button;

		UniButton.Button uniButton;

		void Awake()
		{
			button = GetComponent<Button>();
			uniButton = GetComponent<UniButton.Button>();

			if(button != null)
				button.onClick.AddListener(OnClick);

			if(uniButton != null)
				uniButton.onClick += OnClick;
		}

		void OnDestroy()
		{
			if(button != null)
				button.onClick.RemoveListener(OnClick);

			if(uniButton != null)
				uniButton.onClick -= OnClick;
		}

		void OnClick()
		{
			if(sound.Valid == false)
			{
				Debug.LogWarning("Sound not valid : " + sound + " Played by " + this);
				return;
			}

			AudioManager.Instance.PlaySound(sound);
		}
	}
}
