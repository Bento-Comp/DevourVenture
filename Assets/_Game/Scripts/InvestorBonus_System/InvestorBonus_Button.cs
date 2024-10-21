using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniUI;

public class InvestorBonus_Button : MenuButton
{
	public static System.Action OnInvestorBonusButtonPressed;

	public override void OnClick()
	{
		OnInvestorBonusButtonPressed?.Invoke();
	}
}
