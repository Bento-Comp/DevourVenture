using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniUI;

public class ActiveSkill_PopularDishSelection_ButtonSelect : MenuButton
{
	public System.Action OnPopularDishSelectionButtonPressed;



	public override void OnClick()
	{
		OnPopularDishSelectionButtonPressed?.Invoke();
	}
}
