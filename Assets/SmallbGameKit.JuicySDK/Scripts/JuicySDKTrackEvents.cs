using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameFramework;
using GameFramework.SimpleGame;

using Juicy;

namespace SmallbGameKit
{
	[AddComponentMenu("SmallbGameKit/JuicySDK/JuicySDKTrackEvents")]
	public class JuicySDKTrackEvents : GameBehaviour
	{
		protected override void OnGameStart()
		{
			base.OnGameStart();
			
			if(LevelManager.UseLeveling)
			{
				JuicySDK.NotifyGameStart(LevelManager.LevelIndex_RawAndContinuous);
			}
			else
			{
				JuicySDK.NotifyGameStart(Game.Instance.GameCount);
			}
		}

		protected override void OnAwake()
		{
			base.OnAwake();
		
			ContinueManager.onGameOverConfirmed += OnGameOverConfirmed;
		}

		protected override void OnAwakeEnd()
		{
			base.OnAwakeEnd();

			ContinueManager.onGameOverConfirmed -= OnGameOverConfirmed;
		}

		protected override void OnLevelCompleted(bool success)
		{
			base.OnLevelCompleted(success);

			if(LevelManager.UseLeveling == false)
			{
				success = ScoreManager.Instance.HighScoreBeaten;
			}

			JuicySDK.NotifyGameEnd(ScoreManager.Instance.Score, success);
		}

		void OnGameOverConfirmed()
		{
			if(GameScreenButton.Instance.levelCompletedScreenAfterGameOver)
				return;

			if(LevelManager.UseLeveling)
			{
				JuicySDK.NotifyGameEnd(ScoreManager.Instance.Score, false);
			}
			else
			{
				JuicySDK.NotifyGameEnd(ScoreManager.Instance.Score);
			}
		}

		protected override void OnInterlude()
		{
			base.OnInterlude();

			if(LevelManager.UseLeveling)
			{
				JuicySDK.NotifyGameEnd(ScoreManager.Instance.Score, true);
			}
			else
			{
				JuicySDK.NotifyGameEnd(ScoreManager.Instance.Score);
			}
		}

		protected override void OnInterludeEnd()
		{
			base.OnInterludeEnd();

			if(LevelManager.UseLeveling)
			{
				JuicySDK.NotifyGameStart(LevelManager.LevelIndex_RawAndContinuous);
			}
			else
			{
				JuicySDK.NotifyGameStart(-1);
			}
		}
	}
}
