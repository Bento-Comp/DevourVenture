using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_SpriteSequence")]
	public class SkinItem_SpriteSequence : SkinItem_SpriteBase
	{
		public enum ESequenceMode
		{
			Clamp,
			Repeat,
			PingPong
		}

		public ESequenceMode sequenceMode;

		public int startLoopIndex = 0;

		public int endSize = 0;

		[SerializeField]
		List<Sprite> sprites = new List<Sprite>();

		public override Sprite GetSprite(int index = 0, int count = 1)
		{
			if(count <= 0)
			{
				index = 0;
			}
			else
			{
				index = Mathf.Clamp(index, 0, count - 1);

				int valuesCount = sprites.Count;

				if(valuesCount <= 0)
				{
					return null;
				}

				int endLoopIndex = count - 1 - endSize;
				if(endLoopIndex < startLoopIndex)
				{
					endLoopIndex = startLoopIndex;
				}
				int endValuesLoopIndex = valuesCount - endSize - 1;
				if(index < startLoopIndex)
				{
				}
				else if(index > endLoopIndex)
				{
					int endLoopOffsetIndex = index - endLoopIndex;
					index = endValuesLoopIndex + endLoopOffsetIndex;
				}
				else
				{
					int valuesLoopSize = endValuesLoopIndex - startLoopIndex + 1;
					switch(sequenceMode)
					{
					case ESequenceMode.Clamp:
						{
							index = Mathf.Clamp(index - startLoopIndex, 0, valuesLoopSize - 1) + startLoopIndex;
						}
						break;

					case ESequenceMode.Repeat:
						{
							index = Mathf.FloorToInt(Mathf.Repeat(index - startLoopIndex, valuesLoopSize)) + startLoopIndex;	
						}
						break;

					case ESequenceMode.PingPong:
						{
							index = Mathf.RoundToInt(Mathf.PingPong(index - startLoopIndex, valuesLoopSize - 1)) + startLoopIndex;	
						}
						break;
					}
				}

				#if UNITY_EDITOR
				if(index < 0 || index >= valuesCount)
				{
					Debug.LogError("SkinItem_SpriteSequence : index out of range : index = " + index + " | valuesCount = " + valuesCount + " | count = " + count);
				}
				#endif
			}

			return sprites[index];
		}
	}
}
