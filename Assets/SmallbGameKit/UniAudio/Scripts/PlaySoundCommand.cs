using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniAudio
{
	[Serializable]
	public class PlaySoundCommand
	{
		public AudioClip audioClip;

		public float volume = 1.0f;

		public bool setAndPlay = false;

		public string audioSourceName;

		public bool loop = false;

		public float pitch = 1.0f;

		public SoundLayer soundlayer;

		public SoundAsset fallbackSoundAsset;

		public float Volume
		{
			get
			{
				if(soundlayer == null)
					return volume;

				return soundlayer.Volume * volume;
			}
		}

		public void Clear()
		{
			fallbackSoundAsset = null;
			audioClip = null;
			volume = 1.0f;
			setAndPlay = false;
			audioSourceName = "";
			loop = false;
			pitch = 1.0f;
		}

		public PlaySoundCommand()
		{
			Clear();
		}

		public void UpdatePlaySoundCommand(PlaySoundCommand playSoundCommand)
		{
			if(audioClip == null && fallbackSoundAsset != null)
			{
				fallbackSoundAsset.Sound.UpdatePlaySoundCommand(playSoundCommand);
			}
			else
			{
				playSoundCommand.fallbackSoundAsset = null;
				playSoundCommand.audioClip = audioClip;
				playSoundCommand.volume = volume;
				playSoundCommand.audioSourceName = audioSourceName;
				playSoundCommand.setAndPlay = setAndPlay;
				playSoundCommand.loop = loop;
				playSoundCommand.soundlayer = soundlayer;
			}
		}
	}
}
