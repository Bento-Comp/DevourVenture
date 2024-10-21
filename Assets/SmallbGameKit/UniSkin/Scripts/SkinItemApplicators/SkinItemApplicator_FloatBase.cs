using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_FloatBase")]
	public class SkinItemApplicator_FloatBase : SkinItemApplicatorBase
	{
		public int index = 0;

		public int count = 1;

		protected override void OnSkinChange()
		{
			SkinItem_FloatBase skinItem = GetSkinItem<SkinItem_FloatBase>(skinItemName);

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

			OnFloatChange(skinItem.GetFloat(index, count));
		}

		protected virtual void OnFloatChange(float value)
		{
		}
	}
}
