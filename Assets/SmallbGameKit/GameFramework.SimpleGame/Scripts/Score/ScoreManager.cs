using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(-32000)]
	[AddComponentMenu("GameFramework/ScoreManager")]
	public class ScoreManager : GameFramework.GameBehaviour
	{
		public static System.Action onScoreChange;

		public static System.Action<int> onAddScore;

		public static System.Action onBestScoreChange;

		public bool saveBestScore = true;

		public string scoreDisplayPrefix;

		public string scoreDisplaySuffix;

		int score;

		bool highScoreBeaten;

		static string bestScore_saveKey = "BestScore";

		static ScoreManager instance;

		public static ScoreManager Instance
		{
			get
			{
				return instance;
			}
		}

		public string ScoreDisplayString => GetScoreDisplayString(Score);

		public string BestScoreDisplayString => GetScoreDisplayString(BestScore);

		public int Score
		{
			get
			{ 
				return score;
			}
		}

		public int BestScore
		{
			get
			{
				return _BestScore;
			}
		}

		public bool HighScoreBeaten
		{
			get
			{
				return highScoreBeaten;
			}
		}

		protected virtual string BestScoreSaveKey
		{
			get
			{
				return bestScore_saveKey;
			}
		}

		int _Score
		{
			get
			{
				return score;
			}

			set
			{
				int changeAmount = value - score;

				score = value;

				if(saveBestScore && score > _BestScore)
				{
					_BestScore = score;
					highScoreBeaten = true;
				}
					
				if(onScoreChange != null)
					onScoreChange();

				if(changeAmount > 0)
				{
					onAddScore?.Invoke(changeAmount);
				}
			}
		}

		int _BestScore
		{
			get
			{
				return PlayerPrefs.GetInt(BestScoreSaveKey, 0);
			}

			set
			{
				PlayerPrefs.SetInt(BestScoreSaveKey, value);
				OnBestScoreChange();
			}
		} 

		public void AddScore(int value)
		{
			_Score += value;
		}

		public void SetScoreIfBetterThanCurrent(int newScore)
		{
			if(newScore < _Score)
				return;

			_Score = newScore;
		}

		public virtual string GetScoreDisplayString(int score)
		{
			return scoreDisplayPrefix + score.ToString() + scoreDisplaySuffix;
		}

		protected void NotifyBestScoreChange()
		{
			OnBestScoreChange();
		}

		protected override void OnLoadGame()
		{
			_Score = 0;
			highScoreBeaten = false;
		}

		protected override void OnAwake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}
		
		protected override void OnAwakeEnd()
		{
			if(instance == this)
			{
				instance = null;
			}
		}
		void OnBestScoreChange()
		{
			if(onBestScoreChange != null)
				onBestScoreChange();
		}
	}
}