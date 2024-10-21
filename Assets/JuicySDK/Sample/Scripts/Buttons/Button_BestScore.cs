using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_BestScore")]
	public class Button_BestScore : MonoBehaviour
	{
		public string prefix = "Best Score = ";
		
		public Text textComponent;
		
		void Update()
		{
			UpdateDisplay();
		}

		void UpdateDisplay()
		{
			textComponent.text = prefix + JuicySDK.BestScore;
		}
	}
}