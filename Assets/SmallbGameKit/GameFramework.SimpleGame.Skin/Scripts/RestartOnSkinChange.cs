using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniSkin;

namespace GameFramework.SimpleGame.Skin
{
	[AddComponentMenu("GameFramework/SimpleGame/Skin/RestartOnSkinChange")]
	public class RestartOnSkinChange : MonoBehaviour
	{
		void Awake()
		{
			SkinManager.onSkinChange += OnSkinChange;
		}

		void OnDestroy()
		{
			SkinManager.onSkinChange -= OnSkinChange;
		}

		void OnSkinChange(int skinIndex)
		{
			Game.Instance.DoRestart();
		}
	}
}
