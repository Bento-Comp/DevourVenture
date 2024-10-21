using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace UniAudio
{
	[CustomEditor(typeof(SoundAsset))]
	[CanEditMultipleObjects()]
	public class SoundAsset_Inspector : Editor 
	{
		List<string> lastPlayInfos = new List<string>();
		List<Editor_PreviewSoundController> previewControllers = new List<Editor_PreviewSoundController>();

		public void OnDisable()
		{
			StopSounds();
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			for(int i = previewControllers.Count - 1; i >= 0; --i)
			{
				if(previewControllers[i] == null)
					previewControllers.RemoveAt(i);
			}

			if(GUILayout.Button("Play Sound"))
			{
				//lastPlayInfos.Clear();
				foreach(SoundAsset soundAsset in targets)
				{
					previewControllers.Add(AudioEditorUtility.PlaySound(soundAsset));
					lastPlayInfos.Add("Play : " + soundAsset.Sound.LastPlaySoundCommand.audioClip + " at volume " + soundAsset.Sound.LastPlaySoundCommand.Volume);
				}
			}

			if(previewControllers.Count > 0)
			{
				if(GUILayout.Button("Stop Sounds"))
				{
					StopSounds();
				}
			}

			for(int i = lastPlayInfos.Count - 1; i >= 0; --i)
			{
				GUILayout.Label(lastPlayInfos[i]);
			}
		}

		void StopSounds()
		{
			foreach(Editor_PreviewSoundController previewController in previewControllers)
			{
				DestroyImmediate(previewController.gameObject);
			}
			previewControllers.Clear();
		}
	}
}