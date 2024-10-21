using UnityEngine;
using System.Collections;

using UniUI;

namespace UniPurchase
{
	[AddComponentMenu("UniPurchase/BuyProductButton")]
	public class BuyProductButton : MenuButton
	{
		public string productId;

		public override void OnClick()
		{
			PurchaseManager.Instance.BuyProduct(productId);
		}
	}
}