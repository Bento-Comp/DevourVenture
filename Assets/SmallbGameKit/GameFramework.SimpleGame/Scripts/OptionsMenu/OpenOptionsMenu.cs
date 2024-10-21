using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/OpenOptionsMenu")]
	public class OpenOptionsMenu : MenuButton
	{
		public OptionsMenu optionsMenu;

		public override void OnClick()
		{
			optionsMenu.ToggleOpen();
		}
	}
}