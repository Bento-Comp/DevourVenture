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
	public class SoundLayer : ScriptableObject
	{
		[SerializeField]
		SoundLayer parent = null;

		[SerializeField]
		float volume = 1.0f;

		public float Volume
		{
			get
			{
				if(parent == null)
					return volume;
				
				return volume * parent.Volume;
			}
		}

		#if UNITY_EDITOR
		[MenuItem("Assets/Create/UniAudio/CreateSoundLayer")]
		[MenuItem("UniAudio/Create/CreateSoundLayer")]
		public static void CreateSoundLayer()
		{
			UniEditor.EditorCreateAssetUtility.CreateScriptableObjectInSelectedProjectFolder<SoundLayer>("New Sound Layer");
		}
		#endif
	}
}
