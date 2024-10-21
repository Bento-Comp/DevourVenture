using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniSkin;

namespace GameFramework.SimpleGame.Skin
{
	[AddComponentMenu("GameFramework/SimpleGame/Skin/SkinItemApplicator_Score_SaveBestScore")]
	public class SkinItemApplicator_Score_SaveBestScore : SkinItemApplicator_BoolBase
	{
		ScoreManager scoreManager;

		protected override void OnBoolChange(bool value)
		{
			if(scoreManager == null)
				scoreManager = GetComponent<ScoreManager>();
			
			scoreManager.saveBestScore = value;
		}
	}
}
