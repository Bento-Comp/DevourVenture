using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace UniSkin
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniSkin/SkinLayer")]
	public class SkinLayer : MonoBehaviour
	{
		public SkinUserBase skinUserOverride;

		public int layerIndex = 1;

		[SerializeField]
		int selectedSkinIndex = 0;

		public List<Skin> skins = new List<Skin>();

		public bool autoFillSkin = true;

		public bool renameSkin = true;

		public int SelectedSkinIndex
		{ 
			get
			{
				return selectedSkinIndex;
			}

			set
			{
				value = ConstrainedIndex(value);
				if(selectedSkinIndex != value)
				{
					selectedSkinIndex = value;
					OnSelectSkinIndexChange();
				}
			}
		}

		public Skin SelectedSkin
		{
			get
			{
				if(skins.Count == 0)
					return null;

				return skins[ConstrainedIndex(selectedSkinIndex)];
			}
		}

		public int SkinCount
		{
			get
			{
				return skins.Count;
			}
		}

		SkinUserBase SkinUser
		{
			get
			{
				if(skinUserOverride != null)
					return skinUserOverride;

				return SkinManager.Instance;
			}
		}

#if UNITY_EDITOR
		void Update()
		{
			AutoFill();

			selectedSkinIndex = ConstrainedIndex(selectedSkinIndex);

			SelectSkin();

			if(Application.isPlaying == false)
			{
				SkinUserBase skinUser = SkinUser;
				if(SkinUser != null)
				{
					EditorUtility.SetDirty(skinUser);
					//EditorSceneManager.MarkSceneDirty(skinUser.gameObject.scene);
				}
			}
		}
#endif

		void OnSelectSkinIndexChange()
		{
			SelectSkin();
		}

		void SelectSkin()
		{
			SkinUserBase skinUser = SkinUser;
			if(skinUser == null)
				return;

			if(skins.Count == 0)
				return;

			skinUser.SetSkin(layerIndex, SelectedSkin);
		}

		void AutoFill()
		{
			if(autoFillSkin == false)
				return;

			skins = new List<Skin>(GetComponentsInChildren<Skin>());

			int index = 0;
			if(renameSkin)
			{
				foreach(Skin skin in skins)
				{
					skin.name = name + " (" + index.ToString("00") + ")";

					++index;
				}
			}
		}

		int ConstrainedIndex(int index)
		{
			return Mathf.Clamp(index, 0, skins.Count - 1);
		}
	}
}
