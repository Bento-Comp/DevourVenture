using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniUI;

public class HideUIArea : MenuButton
{
    public static System.Action onClickHideUIArea;

	public override void OnClick()
	{
		onClickHideUIArea?.Invoke();
	}
}
