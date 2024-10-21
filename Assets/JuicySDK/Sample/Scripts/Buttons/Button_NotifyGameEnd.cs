using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_NotifyGameEnd")]
	public class Button_NotifyGameEnd : ButtonBase
	{
		public InputField nextScoreInputField;
		int val = 0;
		protected override void OnClick()
		{
			int nextScore;
			if(int.TryParse(nextScoreInputField.text, out nextScore) == false)
			{
				nextScore = RandomScore();
			}

			JuicySDK.NotifyGameEnd(nextScore);

			SelectRandomNextScore();
		}

		void Start()
		{
			SelectRandomNextScore();
		}

		int RandomScore()
		{
			return val++;
		}

		void SelectRandomNextScore()
		{
			nextScoreInputField.text = RandomScore().ToString();
		}
	}
}