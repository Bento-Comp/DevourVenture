using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniUI;

public class ActiveSkill_PopularDishSelection_ButtonExit : MenuButton
{
	public static System.Action OnPopularDishSelectionExitButtonPressed;

	public override void OnClick()
	{
		OnPopularDishSelectionExitButtonPressed?.Invoke();
	}
}
