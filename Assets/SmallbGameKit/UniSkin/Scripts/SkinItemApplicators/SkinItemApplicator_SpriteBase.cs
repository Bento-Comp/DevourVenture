using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_SpriteBase")]
	public class SkinItemApplicator_SpriteBase : SkinItemApplicatorBase
	{
		public int index = 0;

		public int count = 1;

		protected override void OnSkinChange()
		{
			SkinItem_SpriteBase skinItem = GetSkinItem<SkinItem_SpriteBase>(skinItemName);

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

			OnSpriteChange(skinItem.GetSprite(index, count));
		}

		protected virtual void OnSpriteChange(Sprite sprite)
		{
		}
	}
}
