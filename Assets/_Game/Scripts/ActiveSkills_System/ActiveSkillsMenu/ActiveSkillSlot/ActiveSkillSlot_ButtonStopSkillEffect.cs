using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniUI;

public class ActiveSkillSlot_ButtonStopSkillEffect : MenuButton
{
	public static System.Action OnStopSkillEffectButtonPressed;

	public override void OnClick()
	{
		OnStopSkillEffectButtonPressed?.Invoke();
	}
}
