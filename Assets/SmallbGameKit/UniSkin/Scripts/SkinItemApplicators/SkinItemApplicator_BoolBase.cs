using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_BoolBase")]
	public class SkinItemApplicator_BoolBase : SkinItemApplicatorBase
	{
		public int index = 0;

		public int count = 1;

		protected override void OnSkinChange()
		{
			SkinItem_BoolBase skinItem = GetSkinItem<SkinItem_BoolBase>(skinItemName);

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

			OnBoolChange(skinItem.GetBool(index, count));
		}

		protected virtual void OnBoolChange(bool value)
		{
		}
	}
}
