using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/CountdownCounter")]
	public class CountdownCounter : MonoBehaviour 
	{
		public Text textComponent;

		void Update()
		{
			int remaining = Mathf.CeilToInt(ContinueManager.Instance.CountdownRemainingTime);
			textComponent.text = remaining.ToString();
		}
	}
}