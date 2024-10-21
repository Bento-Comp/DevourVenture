using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniUI;

public class ActiveSkillSlot_ButtonStartSkillEffect : MenuButton
{
	public static System.Action OnStartSkillEffectButtonPressed;

	public override void OnClick()
	{
		OnStartSkillEffectButtonPressed?.Invoke();
	}
}
