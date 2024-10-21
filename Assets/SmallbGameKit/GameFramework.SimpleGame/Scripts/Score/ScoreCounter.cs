using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/ScoreCounter")]
	public class ScoreCounter : MonoBehaviour 
	{
		public Text textComponent;

		public bool hideZeroScore;

		public bool updateOnScoreChange = true;

		void OnEnable()
		{
			UpdateScore();
		}

		void Awake()
		{
			ScoreManager.onScoreChange += OnScoreChange;
		}

		void OnDestroy()
		{
			ScoreManager.onScoreChange -= OnScoreChange;
		}

		void OnScoreChange()
		{
			if(updateOnScoreChange == false)
				return;

			UpdateScore();
		}

		void UpdateScore()
		{
			int score = ScoreManager.Instance.Score;
			if(score <= 0 && hideZeroScore)
			{
				textComponent.enabled = false;
			}
			else
			{
				textComponent.enabled = true;
				textComponent.text = ScoreManager.Instance.ScoreDisplayString;
			}
		}
	}
}