using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

using UniSkin;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameFramework.SimpleGame.Skin
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GameFramework/SimpleGame/Skin/SkinSelectorsSpawner")]
	public class SkinSelectorsSpawner : MonoBehaviour
	{	
		public SkinList skinList;

		public Transform skinSelectorRoot;

		public SkinSelector skinSelectorPrefab;

		public int skinIndex;

		public bool keepPrefabLink = true;

		[SerializeField]
		List<SkinSelector> skinSelectors = new List<SkinSelector>();

		public List<SkinSelector> SkinSelectors
		{
			get
			{
				return skinSelectors;
			}
		}

		#if UNITY_EDITOR
		public void ForceRebuild()
		{
			foreach(SkinSelector skinSelector in skinSelectors)
			{
				DestroyImmediate(skinSelector.gameObject);
			}
			skinSelectors.Clear();

			UpdateList();
		}

		void Update()
		{
			if(Application.isPlaying)
				return;

			UpdateList();
		}

		void UpdateList()
		{
			int skinCount = skinList.skins.Count;

			for(int i = 0; i < skinCount; ++i)
			{
				SkinSelector skinSelector = null;
				if(i == skinSelectors.Count)
				{
					skinSelectors.Add(null);
				}
				else
				{
					skinSelector = skinSelectors[i];
				}
				if(skinSelector == null)
				{
					skinSelector = CreateSkinSelector();
					skinSelectors[i] = skinSelector;
				}

				List<UniSkin.Skin> skins = new List<UniSkin.Skin>();
				for(int k = 0; k <= skinIndex; ++k)
				{
					skins.Add(null);
				}
				UniSkin.Skin skin = skinList.skins[i];
				skins[skinIndex] = skin;
				skinSelector.Skins = skins;

				skinSelector.name = skinSelectorPrefab.name + "_" + skin.name;
			}

			for(int i = skinSelectors.Count - 1; i >= skinCount; --i)
			{
				DestroyImmediate(skinSelectors[i].gameObject);
				skinSelectors.RemoveAt(i);
			}

			UnityEditor.EditorUtility.SetDirty(this);
		}

		SkinSelector CreateSkinSelector()
		{
			SkinSelector skinSelector;
			if(keepPrefabLink)
			{
				skinSelector = PrefabUtility.InstantiatePrefab(skinSelectorPrefab, skinSelectorRoot) as SkinSelector;
			}
			else
			{
				skinSelector = Instantiate<SkinSelector>(skinSelectorPrefab, skinSelectorRoot, false);
			}
			return skinSelector;
		}
		#endif
	}
}