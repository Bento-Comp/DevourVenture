using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace UniPurchase
{
	[AddComponentMenu("UniPurchase/DestroyWhenRestorePurchasesNotSupported")]
	public class DestroyWhenRestorePurchasesNotSupported : MonoBehaviour
	{
		public GameObject gameObjectToDestroy;

		void Start()
		{
			if(gameObjectToDestroy == null)
				return;
			
			if(PurchaseManager.Instance.RestorePurchasesSupported == false)
				DestroyImmediate(gameObjectToDestroy);
		}
	}
}