using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniSkin;

namespace UniGameMode.Skin
{
	[AddComponentMenu("UniGameMode/Skin/SelectSkinOnGameMode")]
	public class SelectSkinOnGameMode : SkinSelector
	{
		public string gameMode = "default";
		
		protected override void Awake()
		{
			base.Awake();
			GameModeManager.onGameModeChange += OnGameModeChange;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			GameModeManager.onGameModeChange -= OnGameModeChange;
		}

		void OnGameModeChange()
		{
			if(GameModeManager.Instance.IsGameModeEnabled(gameMode))
				SelectSkin();
		}
	}
}
