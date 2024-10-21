using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniAudio
{
	[Serializable]
	public class SoundSequenceAsset : SoundAsset
	{
		public SoundSequence soundSequence;

		public override Sound Sound
		{
			get
			{
				return soundSequence;
			}
		}

		#if UNITY_EDITOR
		[MenuItem("Assets/Create/UniAudio/CreateSoundSequenceAsset")]
		[MenuItem("UniAudio/Create/CreateSoundSequenceAsset")]
		public static void CreateSoundAsset()
		{
			UniEditor.EditorCreateAssetUtility.CreateScriptableObjectInSelectedProjectFolder<SoundSequenceAsset>("New Sound Sequence Asset");
		}
		#endif
	}
}
