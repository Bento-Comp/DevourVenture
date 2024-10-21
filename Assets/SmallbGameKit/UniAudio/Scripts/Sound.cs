using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniAudio
{
	[Serializable]
	public abstract class Sound
	{
		PlaySoundCommand playSoundCommand = new PlaySoundCommand();

		public PlaySoundCommand LastPlaySoundCommand
		{
			get
			{
				return playSoundCommand;
			}
		}

		public PlaySoundCommand NextPlaySoundCommand
		{
			get
			{
				UpdatePlaySoundCommand(playSoundCommand);
				return playSoundCommand;
			}
		}

		public abstract void UpdatePlaySoundCommand(PlaySoundCommand playSoundCommand);
	}
}
