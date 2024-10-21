using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniSkin/SkinItemApplicator_StringBase")]
	public class SkinItemApplicator_StringBase : SkinItemApplicatorBase
	{
		public int index = 0;

		public int count = 1;

		protected override void OnSkinChange()
		{
			SkinItem_StringBase skinItem = GetSkinItem<SkinItem_StringBase>(skinItemName);

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

			OnStringChange(skinItem.GetString(index, count));
		}

		protected virtual void OnStringChange(string value)
		{
		}
	}
}
