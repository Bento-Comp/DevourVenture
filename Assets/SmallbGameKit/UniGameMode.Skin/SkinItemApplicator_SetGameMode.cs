using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniSkin;

namespace UniGameMode.Skin
{
	[AddComponentMenu("UniGameMode/Skin/SkinItemApplicator_SetGameMode")]
	public class SkinItemApplicator_SetGameMode : SkinItemApplicator_StringBase
	{
		GameModeManager gameModeManager;

		protected override void OnStringChange(string value)
		{
			if(gameModeManager == null)
				gameModeManager = GetComponent<GameModeManager>();
			
			gameModeManager.EnableGameMode(value);
		}
	}
}
