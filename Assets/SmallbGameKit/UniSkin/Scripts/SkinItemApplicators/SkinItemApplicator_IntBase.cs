using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_IntBase")]
	public class SkinItemApplicator_IntBase : SkinItemApplicatorBase
	{
		public int index = 0;

		public int count = 1;

		protected override void OnSkinChange()
		{
			SkinItem_IntBase skinItem = GetSkinItem<SkinItem_IntBase>(skinItemName);

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

			OnIntChange(skinItem.GetInt(index, count));
		}

		protected virtual void OnIntChange(int value)
		{
		}
	}
}
