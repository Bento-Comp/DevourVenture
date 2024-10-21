using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UniAudio
{
	[CustomEditor(typeof(SoundSequenceAsset))]
	[CanEditMultipleObjects()]
	public class SoundAssetSequence_Inspector : SoundAsset_Inspector 
	{
	}
}