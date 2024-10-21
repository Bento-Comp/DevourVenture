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
	[AddComponentMenu("UniSkin/Skin")]
	public class Skin : MonoBehaviour
	{
		public Skin parentSkin;

		Dictionary<KeyValuePair<string, Type>, SkinItemBase> skinItems;

		bool dictionaryFilled;
			
		public SkinItemType GetSkinItem<SkinItemType>(string skinItemName) where SkinItemType : SkinItemBase
		{
			if(Application.isPlaying == false)
			{
				FillDictionnary();
			}
			else
			{
				if(skinItems == null)
				{
					FillDictionnary();
				}
			}
			
			SkinItemBase skinItem;
			if(skinItems.TryGetValue(new KeyValuePair<string, Type>(skinItemName, typeof(SkinItemType)), out skinItem))
			{
				return skinItem as SkinItemType;
			}
			else if(parentSkin != null)
			{
				return parentSkin.GetSkinItem<SkinItemType>(skinItemName);
			}

			return null;
		}
			
		void FillDictionnary()
		{
			if(this == null)
				return;
			
			skinItems = new Dictionary<KeyValuePair<string, System.Type>, SkinItemBase>();

			Type skinItemBaseType = typeof(SkinItemBase);
			foreach(SkinItemBase skinItem in GetComponents<SkinItemBase>())
			{
				if(skinItem == null)
					continue;

				Type type = skinItem.GetType();

				while(type != skinItemBaseType)
				{
					skinItems.Add(new KeyValuePair<string, Type>(skinItem.skinItemName, type), skinItem);
					foreach(string alias in skinItem.skinItemAliases)
					{
						skinItems.Add(new KeyValuePair<string, Type>(alias, type), skinItem);
					}

					type = type.BaseType;
				}
			}
		}
	}
}
