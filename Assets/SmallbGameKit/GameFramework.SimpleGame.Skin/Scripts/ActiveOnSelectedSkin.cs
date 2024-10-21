using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UniSkin;

namespace GameFramework.SimpleGame.Skin
{
	[AddComponentMenu("GameFramework/SimpleGame/Skin/ActiveOnSelectedSkin")]
	public class ActiveOnSelectedSkin : MonoBehaviour
	{	
		public SkinSelector skinSelector;

		public UniActivation.Activator activator;

		void Awake()
		{
			SkinManager.onSkinChange += OnSkinChange;
			activator.SelectedIndex = 0;
			OnSkinChange();
		}

		void OnDestroy()
		{
			SkinManager.onSkinChange -= OnSkinChange;
		}

		void OnSkinChange(int skinIndex)
		{
			OnSkinChange();
		}

		void OnSkinChange()
		{
			activator.SelectedIndex = skinSelector.SkinSelected ? 1 : 0;
		}
	}
}