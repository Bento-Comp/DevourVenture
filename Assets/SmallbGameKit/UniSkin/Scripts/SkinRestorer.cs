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
	[AddComponentMenu("UniSkin/SkinRestorer")]
	public class SkinRestorer : MonoBehaviour
	{
		public SkinList skinList;

		public string skinIDFieldName = "NameID";

		public int skinIndex = 0;

		string SelectedSkin_KeySave
		{
			get
			{
				return name + "_SelectedSkin";
			}
		}

		string SkinID
		{
			get
			{
				return PlayerPrefs.GetString(SelectedSkin_KeySave, "");
			}

			set
			{
				PlayerPrefs.SetString(SelectedSkin_KeySave, value);
			}
		}

		public void Save()
		{
			SkinID = GetSkinID(SkinManager.Instance.GetSkin(skinIndex));
		}

		public void Restore()
		{
			string skinID = SkinID;

			foreach(Skin skin in skinList.skins)
			{
				if(GetSkinID(skin) == skinID)
				{
					SkinManager.Instance.SetSkin(skinIndex, skin);
					return;
				}
			}
		}

		string GetSkinID(Skin skin)
		{
			return skin.GetSkinItem<SkinItem_StringBase>(skinIDFieldName).GetString();
		}
	}
}
