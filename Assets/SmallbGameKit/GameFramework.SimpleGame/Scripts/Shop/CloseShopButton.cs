using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/CloseShopButton")]
	public class CloseShopButton : MenuButton
	{
		public string shopName;

		public override void OnClick()
		{
			ShopManager.Instance.CloseShop(shopName);
		}
	}
}