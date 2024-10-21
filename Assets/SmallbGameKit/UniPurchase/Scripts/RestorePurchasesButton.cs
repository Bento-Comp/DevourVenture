using UnityEngine;
using System.Collections;

using UniUI;

namespace UniPurchase
{
	[AddComponentMenu("UniPurchase/RestorePurchasesButton")]
	public class RestorePurchasesButton : MenuButton
	{
		public override void OnClick()
		{
			PurchaseManager.Instance.RestorePurchases();
		}
	}
}