using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniUtilities
{
	public class ArithmeticUtility
	{
		public static int Repeat(int number, int start, int end)
		{
			int length = end - start + 1;
			int offsetFromStart = number - start;
			return FlooredModulo(offsetFromStart, length) + start;
		}

		public static int FlooredModulo(int number, int modulus)
		{
			return (number % modulus + modulus) % modulus;
		}

		public static float FlooredModulo(float number, float modulus)
		{
			return (number % modulus + modulus) % modulus;
		}
	}
}
