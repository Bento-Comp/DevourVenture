using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniUI;

public class LevelSelector_Button_Debug_NextLevel : MenuButton
{
	public static System.Action OnPress_DebugNextLevelButton;

	public override void OnClick()
	{
		OnPress_DebugNextLevelButton?.Invoke();
	}
}
