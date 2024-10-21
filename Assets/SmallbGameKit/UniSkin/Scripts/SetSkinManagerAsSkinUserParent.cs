using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[DefaultExecutionOrder(-31999)]
	[ExecuteAlways()]
	[AddComponentMenu("UniSkin/SetSkinManagerAsSkinUserParent")]
	public class SetSkinManagerAsSkinUserParent : MonoBehaviour
	{
		public SkinUserBase skinUser;

		void Awake()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			skinUser.ParentSkinUser = SkinManager.Instance;
		}

#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			if(skinUser == null)
				return;

			SetSkinParent();
		}
#endif

		void SetSkinParent()
		{
			skinUser.ParentSkinUser = SkinManager.Instance;
		}
	}
}
