using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_FloatGradient")]
	public class SkinItem_FloatGradient : SkinItem_FloatBase
	{
		public enum EGradientType
		{
			ScaledWithCount,
			FixedMax
		}

		public EGradientType gradientType = EGradientType.ScaledWithCount;

		[SerializeField]
		List<float> values = new List<float>{1.0f};

		public int maxIndex = 20;

		public override float GetFloat(int index = 0, int count = 1)
		{
			int max = 0;
			switch(gradientType)
			{
			case EGradientType.ScaledWithCount:
				{
					max = count - 1;
				}
				break;

			case EGradientType.FixedMax:
				{
					max = maxIndex;
				}
				break;
			}

			if(max <= 0)
				max = 0;

			if(values.Count <= 0)
				return 1.0f;

			if(values.Count == 1)
				return values[0];

			float totalPercent = Mathf.Clamp01(Mathf.InverseLerp(0, max, index));

			float indexPercent = totalPercent * (values.Count - 1);

			int indexInt = Mathf.FloorToInt(indexPercent);

			if(indexInt >= values.Count - 1)
			{
				return values[values.Count - 1];
			}

			float betweenPercent = indexPercent - indexInt;

			return Mathf.Lerp(values[indexInt], values[indexInt + 1], betweenPercent);
		}
	}
}
