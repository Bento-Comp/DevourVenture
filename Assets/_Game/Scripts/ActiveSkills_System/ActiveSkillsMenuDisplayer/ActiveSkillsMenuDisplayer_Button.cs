using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniUI;

public class ActiveSkillsMenuDisplayer_Button : MenuButton
{
	public System.Action OnDisplayActiveSkillsMenuButtonPressed;
	public static System.Action OnDisplayActiveSkillsMenuButtonPressed_Global;



	public override void OnClick()
	{
		OnDisplayActiveSkillsMenuButtonPressed?.Invoke();
		OnDisplayActiveSkillsMenuButtonPressed_Global?.Invoke();
	}
}
