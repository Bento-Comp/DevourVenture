using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

using UniSkin;

namespace GameFramework.SimpleGame.Skin
{
	[AddComponentMenu("GameFramework/SimpleGame/Skin/SelectSkinButton")]
	public class SelectSkinButton : MonoBehaviour
	{	
		public SkinSelector skinSelector;

		Button button;

		void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(OnClick); 
		}

		void OnDestroy()
		{
			if(button != null)
				button.onClick.RemoveListener(OnClick);
		}

		void OnClick()
		{
			skinSelector.SelectSkin();
		}
	}
}