using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniAudio
{
	[Serializable]
	public class SoundSequence : Sound
	{
		[Serializable]
		public class SoundProperty
		{
			public PlaySoundCommand sound;
		}
		public ESoundSequenceType sequenceType;

		public List<SoundProperty> sounds = new List<SoundProperty>(){new SoundProperty()};

		public string audioSourceName;

		public float masterVolume = 1.0f;

		public SoundLayer soundLayer;

		int currentIndex;

		List<int> shuffledIndices = new List<int>();

		int currentShuffleIndex = -1;

		public override void UpdatePlaySoundCommand(PlaySoundCommand playSoundCommand)
		{
			playSoundCommand.Clear();

			currentIndex = GetNextAudioClipIndex();
			SoundProperty soundProperty = sounds[currentIndex];

			soundProperty.sound.UpdatePlaySoundCommand(playSoundCommand);
		
			if(audioSourceName != "" && playSoundCommand.audioSourceName == "")
				playSoundCommand.audioSourceName = audioSourceName;

			if(soundLayer != null && playSoundCommand.soundlayer == null)
				playSoundCommand.soundlayer = soundLayer;

			playSoundCommand.volume *= masterVolume;
		}

		public int GetNextAudioClipIndex()
		{
			switch(sequenceType)
			{
				case ESoundSequenceType.Sequential:
				{
					return (currentIndex + 1) % sounds.Count;
				}

				case ESoundSequenceType.Random:
				{
					return UnityEngine.Random.Range(0, sounds.Count);
				}

				case ESoundSequenceType.Shuffle:
				{
					if(shuffledIndices.Count <= 0 || shuffledIndices.Count != sounds.Count || currentShuffleIndex >= shuffledIndices.Count)
					{
						Shuffle(currentShuffleIndex);	
					}

					currentIndex = shuffledIndices[currentShuffleIndex];
					++currentShuffleIndex;

					return currentIndex;
				}
			}

			return 0;
		}

		void Shuffle(int lastIndex)
		{
			shuffledIndices.Clear();
			currentShuffleIndex = 0;

			List<int> availableIndices = new List<int>();
			for(int i = 0; i < sounds.Count; ++i)
			{
				availableIndices.Add(i);
			}

			while(availableIndices.Count > 0)
			{
				int indexToExtract = UnityEngine.Random.Range(0, availableIndices.Count);
				shuffledIndices.Add(availableIndices[indexToExtract]);
				availableIndices.RemoveAt(indexToExtract);
			}
		}
	}
}
