using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/Debug_PlayTime")]
	public class Debug_PlayTime : MonoBehaviour 
	{
		public Text textComponent;

		void Update()
		{
			UpdateText();
		}

		void UpdateText()
		{
			string text = Game.Instance.PlayTime.ToString("0:00");

			textComponent.text = text;
		}
	}
}