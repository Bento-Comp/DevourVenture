using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_NotifyGameStart")]
	public class Button_NotifyGameStart : ButtonBase
	{
		protected override void OnClick()
		{
			JuicySDK.NotifyGameStart(-1);
		}
	}
}