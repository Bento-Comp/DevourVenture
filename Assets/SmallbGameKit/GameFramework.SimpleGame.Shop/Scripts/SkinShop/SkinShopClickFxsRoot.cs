using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UniSkin;
using UniMoney;
using UniHapticFeedback;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(32000)]
	[AddComponentMenu("GameFramework/SimpleGame/SkinShopClickFxsRoot")]
	public class SkinShopClickFxsRoot : MonoBehaviour
	{
		void OnEnable()
		{
			SkinShopManager.Instance.clickFxRoot = transform;
		}
	}
}