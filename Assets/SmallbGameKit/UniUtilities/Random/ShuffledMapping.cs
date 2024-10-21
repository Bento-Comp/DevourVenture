using UnityEngine;
using System.Collections.Generic;

namespace UniUtilities
{
	[System.Serializable]
	public class ShuffledIntMapping
	{
		[SerializeField]
		List<int> shuffledInts;

		public int GetMappedIndex(int index, int offset)
		{
			int localIndex = index - offset;

			if(localIndex < 0 || shuffledInts == null || shuffledInts.Count == 0)
				return index;

			return shuffledInts[localIndex%shuffledInts.Count];
		}

		public ShuffledIntMapping(int loopCount, List<int> indices)
		{
			if(loopCount <= 0)
				return;

			shuffledInts = new List<int>();

			int lastIndexAdded;
			List<int> shuffledList;

			shuffledList = CreateShuffledList(indices);
			FillIndicesList(shuffledInts, shuffledList, out lastIndexAdded);
			for(int i = 1; i < loopCount; ++i)
			{
				shuffledList = CreateShuffledList(indices, lastIndexAdded);
				FillIndicesList(shuffledInts, shuffledList, out lastIndexAdded);
			}
		}

		public void Clear()
		{
			shuffledInts.Clear();
		}

		void FillIndicesList(List<int> indicesList, List<int> indicesToAdd, out int lastIndexAdded)
		{
			lastIndexAdded = 0;
			for(int i = 0; i < indicesToAdd.Count; ++i)
			{
				lastIndexAdded = indicesToAdd[i];
				indicesList.Add(lastIndexAdded);
			}
		}

		List<int> CreateShuffledList(List<int> indices)
		{
			List<int> shuffledList = new List<int>(indices);

			shuffledList.Shuffle<int>();

			return shuffledList;
		}

		List<int> CreateShuffledList(List<int> indices, int forbiddenIndex)
		{
			List<int> shuffledList = new List<int>(indices);

			shuffledList.Remove(forbiddenIndex);

			shuffledList.Shuffle<int>();

			return shuffledList;
		}
	}
}