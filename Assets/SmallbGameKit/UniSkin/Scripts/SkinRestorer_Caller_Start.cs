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
	[AddComponentMenu("UniSkin/SkinRestorer_Caller_Start")]
	public class SkinRestorer_Caller_Start : MonoBehaviour
	{
		public SkinRestorer skinRestorer;

		public int skinIndex;

		void Awake()
		{
			SkinManager.onSkinChange += OnSkinChange;
		}

		void Start()
		{
			skinRestorer.Restore();
		}

		void OnDestroy()
		{
			SkinManager.onSkinChange -= OnSkinChange;
		}

		void OnSkinChange(int skinIndex)
		{
			if(this.skinIndex != skinIndex)
				return;

			skinRestorer.Save();
		}
	}
}
