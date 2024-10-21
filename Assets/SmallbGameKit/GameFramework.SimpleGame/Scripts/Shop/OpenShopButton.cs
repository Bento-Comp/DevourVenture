using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/OpenShopButton")]
	public class OpenShopButton : MenuButton
	{
		public string shopName;
		
		public override void OnClick()
		{
			ShopManager.Instance.OpenShop(shopName);
		}
	}
}