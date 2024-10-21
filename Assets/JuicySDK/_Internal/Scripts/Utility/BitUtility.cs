using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuicyInternal
{
    public class BitUtility
    {
        public static BitArray Combine (params BitArray[] array)
        {
            List<bool> boolList = new List<bool>();
            for(int i=0; i<array.Length; i++)
            {
                for(int j=0; j < array[i].Length; j++)
                {
                    boolList.Add(array[i][j]);
                }
            }
            return new BitArray(boolList.ToArray());
        }

        public static BitArray GetBitsFromInt(int value, int arrayLength)
        {
            int maxValue = (int)Mathf.Pow(2, arrayLength);
            if (value >= maxValue)
               return new BitArray(arrayLength,true);

            BitArray result = new BitArray(arrayLength);
            BitArray conversion = new BitArray(new int[] { value });

            for(int i=0; i<arrayLength; i++)
                result[i] = conversion[i];

            return result;
        }

        public static int GetIntFromBit(BitArray array)
        {
            int value = 0;
            int length = Mathf.Min(array.Length,32);
            for(int i=0; i< length; i++)
                value += array[i] ? (int)Mathf.Pow(2, i) : 0;
            return value;
        }
    }
}
