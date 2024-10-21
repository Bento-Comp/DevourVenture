using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniSkin/SkinSelector")]
	public class SkinSelector : SkinUserBase
	{
		public bool SkinSelected
		{
			get
			{
				SkinManager skinManager = SkinManager.Instance;

				List<Skin> skins = Skins;
				for(int i = 0; i < skins.Count && i < skinManager.SkinLayerCount; ++i)
				{
					Skin skin = skins[i];

					if(skin == null)
						continue;

					if(skinManager.GetSkin(i) != skin)
						return false;
				}

				return true;
			}
		}

		public void SelectSkin()
		{
			SkinManager skinManager = SkinManager.Instance;

			CopySkinsTo(skinManager);
		}

		public override SkinItemType GetSkinItem<SkinItemType>(string skinItemName)
		{
			SkinItemType skinItem = base.GetSkinItem<SkinItemType>(skinItemName);
			if(skinItem == null)
			{
				#if UNITY_EDITOR
				if(Application.isPlaying == false && SkinManager.Instance == null)
				{
				}
				else
				#endif
				{
					SkinUserBase defaultSkinUserBase = SkinManager.Instance.ParentSkinUser;
					if(defaultSkinUserBase != null)
						skinItem = defaultSkinUserBase.GetSkinItem<SkinItemType>(skinItemName);
				}
			}

			return skinItem;
		}

		protected override bool TryGetSkinItem<SkinItemType>(string skinItemName, Skin skin, int skinIndex, out SkinItemType skinItem)
		{
			if(skin == null)
			{
				#if UNITY_EDITOR
				if(Application.isPlaying == false && SkinManager.Instance == null)
				{
				}
				else
				#endif
				{
					skin = SkinManager.Instance.GetSkin(skinIndex);
				}
			}

			return base.TryGetSkinItem<SkinItemType>(skinItemName, skin, skinIndex, out skinItem);
		}
	}
}
