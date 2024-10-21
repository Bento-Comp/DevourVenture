using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/RestartButton")]
	public class RestartButton : MenuButton
	{
		public override void OnClick()
		{
			Game.Instance.DoRestart();
		}
	}
}