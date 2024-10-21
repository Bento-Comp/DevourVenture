using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_MaterialInstance_Color")]
	public class SkinItemApplicator_MaterialInstance_Color : SkinItemApplicatorBase
    {
		public enum AlphaMode
		{
			Replace,
			DontReplace
		}

		public MaterialInstance materialInstance;

		public AlphaMode alphaMode = AlphaMode.Replace;

		public int index = 0;

		public int count = 1;

		protected override void OnSkinChange()
		{
			SkinItem_ColorBase skinItem = GetSkinItem<SkinItem_ColorBase>(skinItemName);

			if(skinItem == null)
			{
				materialInstance.ResetToShareMaterial();
				return;
			}

			OnColorChange(skinItem.GetColor(index, count));
		}

		void OnColorChange(Color color)
		{
            if(materialInstance != null)
			{
				Color materialColor = materialInstance.Color;

				switch(alphaMode)
				{
					case AlphaMode.Replace:
						{
							materialColor = color;
						}
						break;

					case AlphaMode.DontReplace:
						{
							materialColor.r = color.r;
							materialColor.g = color.g;
							materialColor.b = color.b;
						}
						break;
				}

			    materialInstance.Color = materialColor;
			}
		}
	}
}
