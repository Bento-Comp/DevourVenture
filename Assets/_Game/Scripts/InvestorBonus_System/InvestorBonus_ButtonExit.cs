using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniUI;

public class InvestorBonus_ButtonExit : MenuButton
{
	public static System.Action OnInvestorBonusExitButtonPressed;

	public override void OnClick()
	{
		OnInvestorBonusExitButtonPressed?.Invoke();
	}
}
