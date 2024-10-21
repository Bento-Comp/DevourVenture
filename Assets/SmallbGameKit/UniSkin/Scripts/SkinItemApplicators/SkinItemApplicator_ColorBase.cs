using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_ColorBase")]
	public abstract class SkinItemApplicator_ColorBase : SkinItemApplicatorBase
	{
		public enum AlphaMode
		{
			Replace,
			DontReplace
		}

		public int index = 0;

		public int count = 1;

		public AlphaMode alphaMode = AlphaMode.Replace;

		protected abstract Color CurrentColor
		{
			get;
		}

		protected override void OnSkinChange()
		{
			SkinItem_ColorBase skinItem = GetSkinItem<SkinItem_ColorBase>(skinItemName);

			#if UNITY_EDITOR
			if(Application.isPlaying == false && skinItem == null)
				return;

			if(Application.isPlaying)
			{
				if(skinItem == null)
				{
					Debug.LogError("Skin not found : skinItemName = " + skinItemName + " | this : " + this);
				}
			}
			#endif

			OnColorChange( GetColorOverride( skinItem.GetColor(index, count) ) );
		}

		protected virtual void OnColorChange(Color color)
		{
		}

		Color GetColorOverride(Color newColor)
		{
			Color color;
			switch(alphaMode)
			{
				case AlphaMode.Replace:
					{
						color.r = newColor.r;
						color.g = newColor.g;
						color.b = newColor.b;
						color.a = newColor.a;
					}
					break;

				case AlphaMode.DontReplace:
				default:
					{
						color.r = newColor.r;
						color.g = newColor.g;
						color.b = newColor.b;
						color.a = CurrentColor.a;
					}
					break;
			}

			return color;
		}
	}
}
