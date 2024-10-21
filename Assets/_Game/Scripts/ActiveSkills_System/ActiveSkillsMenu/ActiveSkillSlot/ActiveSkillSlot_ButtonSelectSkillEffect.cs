using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniUI;

public class ActiveSkillSlot_ButtonSelectSkillEffect : MenuButton
{
	public static System.Action OnSelectSkillEffectButtonPressed;

	public override void OnClick()
	{
		OnSelectSkillEffectButtonPressed?.Invoke();
	}
}
