using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniAudio
{
	[Serializable]
	public abstract class SoundAsset : ScriptableObject
	{
		public abstract Sound Sound
		{
			get;
		}
	}
}
