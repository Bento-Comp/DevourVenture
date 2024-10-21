using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/ContinueButton")]
	public class ContinueButton : MenuButton 
	{
		public override void OnClick()
		{
			ContinueManager.Instance.AskForContinue();
		}
	}
}