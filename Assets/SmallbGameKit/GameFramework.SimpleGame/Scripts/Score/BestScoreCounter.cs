using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/BestScoreCounter")]
	public class BestScoreCounter : MonoBehaviour 
	{
		public Text textComponent;

		bool started;

		void OnEnable()
		{
			if(started == false)
				return;
			
			OnBestScoreChange();
		}

		void Start()
		{
			started = true;
			ScoreManager.onBestScoreChange += OnBestScoreChange;

			UpdateBestScore();
		}

		void OnDestroy()
		{
			ScoreManager.onBestScoreChange -= OnBestScoreChange;
		}

		void OnBestScoreChange()
		{
			UpdateBestScore();
		}

		void UpdateBestScore()
		{
			textComponent.text = ScoreManager.Instance.BestScoreDisplayString;
		}
	}
}