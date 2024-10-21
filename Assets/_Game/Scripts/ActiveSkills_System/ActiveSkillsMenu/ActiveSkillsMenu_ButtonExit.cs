using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniUI;

public class ActiveSkillsMenu_ButtonExit : MenuButton
{
	public static System.Action OnActiveSkillsExitButtonPressed;

	public override void OnClick()
	{
		OnActiveSkillsExitButtonPressed?.Invoke();
	}
}
