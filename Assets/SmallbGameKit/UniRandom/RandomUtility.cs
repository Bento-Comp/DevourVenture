using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace UniRandom
{
	public static class RandomUtility
	{
		// get a random value between 0.0f and 1.0f
		static public float RandomValue(float seed)
		{
			Random.State state = SetRandomSeed(seed);

			float valuePercent = Random.value;

			RestoreRandomState(state);

			return valuePercent;
		}

		static public float RandomValue(float seed, float min, float max)
		{
			Random.State state = SetRandomSeed(seed);

			float valuePercent = Random.Range(min, max);

			RestoreRandomState(state);

			return valuePercent;
		}

		static public Vector2 RandomValueOnCircle()
		{
			float randomAngle = Random.Range(0.0f, Mathf.PI * 2.0f);
			return new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
		}

		static public Vector2 RandomValueOnArc(float arcMiddleAngle, float arcHalfAmplitudeAngle)
		{
			float randomAngleRadians = (arcMiddleAngle + Random.Range(-arcHalfAmplitudeAngle, arcHalfAmplitudeAngle)) * Mathf.Deg2Rad;

			return new Vector2(Mathf.Cos(randomAngleRadians), Mathf.Sin(randomAngleRadians));
		}

		static public Vector3 RandomValueOnCone(Vector3 axis, float angle)
		{
			float randomPitch = Random.Range(-angle, angle);
			float randomYaw = Random.Range(0.0f, 360.0f);

			Quaternion rotationQuaternion = Quaternion.Euler(randomPitch, randomYaw, 0.0f);

			Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotationQuaternion);

			return rotationMatrix.MultiplyVector(axis);
		}

		// return the random state before the operation
		static public Random.State SetRandomSeed(float seed)
		{
			Random.State state = Random.state;

			Random.InitState(BijectiveFloatToInt(seed));

			return state;
		}

		static public void RestoreRandomState(Random.State randomState)
		{
			Random.state = randomState;
		}

		static int BijectiveFloatToInt(float valueFloat)
		{
			byte[] valueBytes = System.BitConverter.GetBytes(valueFloat);
			int valueInt = System.BitConverter.ToInt32(valueBytes, 0);

			//Debug.Log("float = " + valueFloat + " uint = " + valueInt);

			return valueInt;
		}

		public static T NextShuffled<T>(this IList<T> list, ref int index)
		{
			ShuffleAndRepeatIfIndexOutOfRange(list, ref index);

			T item = default(T);
			if(index >= 0 || index < list.Count)
			{
				item = list[index];
			}

			++index;

			ShuffleAndRepeatIfIndexOutOfRange(list, ref index);

			return item;
		}

		public static void Shuffle<T>(this IList<T> list)  
		{  
			int n = list.Count;  
			while(n > 1)
			{
				n--;
				int k = Random.Range(0, n+1);
				T value = list[k];  
				list[k] = list[n];  
				list[n] = value;
			}  
		}

		static void ShuffleAndRepeatIfIndexOutOfRange<T>(IList<T> list, ref int index)
		{
			if(index < 0 || index >= list.Count)
			{
				list.Shuffle();
				index = 0;	
			}
		}

		public static List<int> CreateAvailableIndices(int count, HashSet<int> indicesToIgnore)
		{
			List<int> availableIndices = new List<int>();
			for(int i = 0; i < count; ++i)
			{
				if(indicesToIgnore.Contains(i))
						continue;

				availableIndices.Add(i);
			}

			return availableIndices;
		}

		public static List<int> CreateAvailableIndices(int count)
		{
			List<int> availableIndices = new List<int>();
			for(int i = 0; i < count; ++i)
			{
				availableIndices.Add(i);
			}

			return availableIndices;
		}

		public static T GetRandomElement<T>(IList<T> list)
		{
			return GetRandomElement(list, out int randomIndex);
		}

		public static T GetRandomElement<T>(IList<T> list, out int randomIndex)
		{
			randomIndex = 0;

			if(list.Count <= 0)
				return default(T);

			randomIndex = Random.Range(0, list.Count);
			return list[randomIndex];
		}

		public static T GetRandomElement<T>(IList<T> list, HashSet<int> indicesToIgnore, out int randomIndex)
		{
			List<int> availableIndices = CreateAvailableIndices(list.Count, indicesToIgnore);

			randomIndex = GetRandomElement<int>(availableIndices);

			return list[randomIndex];
		}

		public static int SelectAndExtractRandomIndex(List<int> availableIndices)
		{
			int randomAvailableListIndex;
			int selectedIndex = GetRandomElement<int>(availableIndices, out randomAvailableListIndex);

			availableIndices.RemoveAt(randomAvailableListIndex);

			return selectedIndex;
		}

		public static void CopyRandomElementsWithoutDuplicate<T>(IList<T> source,
			IList<T> destination, int count)
		{
			int sourceCount = source.Count;

			if(count > sourceCount)
				count = sourceCount;

			if(count <= 0)
				return;

			List<int> availableIndices = CreateAvailableIndices(sourceCount);

			for(int i = 0; i < count; ++i)
			{
				int selectedIndex = SelectAndExtractRandomIndex(availableIndices);
				destination.Add(source[selectedIndex]);
			}
		}

		public static void AddRandomIndicesWithoutDuplicate(List<int> destination, int count, int maxValue)
		{
			List<int> availableIndices = CreateAvailableIndices(maxValue);

			for(int i = 0; i < count && i < maxValue; ++i)
			{
				int selectedIndex = SelectAndExtractRandomIndex(availableIndices);
				destination.Add(selectedIndex);
			}
		}
	}
}
