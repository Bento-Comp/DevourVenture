using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_BuyProduct")]
	public class Button_BuyProduct : ButtonBase
	{
		public string prefix = "Buy Consumable : ";

		public string productId = "consumable01";

		public Text textComponent;

		string productCount_prefix_saveKey = "JuicySDKSample_ProductCount";
		string ProductCount_saveKey
		{
			get
			{
				return productCount_prefix_saveKey + "_" + productId;
			}
		}
		int ProductCount
		{
			get
			{
				return PlayerPrefs.GetInt(ProductCount_saveKey, 0);
			}

			set
			{
				PlayerPrefs.SetInt(ProductCount_saveKey, value);
			}
		}

		protected override void OnClick()
		{
			JuicySDK.BuyProduct(productId);
		}

		void Start()
		{
			JuicySDK.AddProductDeliveryListener(OnProductDelivery);
			UpdateDisplay();
		}

		void OnDestroy()
		{
			JuicySDK.RemoveProductDeliveryListener(OnProductDelivery);
		}

		void OnProductDelivery(ProductSummary productSummary)
		{
			if(productSummary.productId == productId)
			{
				switch(productSummary.productType)
				{
					case ProductType.Consumable:
					{
						++ProductCount;
						
					}
					break;

					case ProductType.NonConsumable:
					{
						ProductCount = 1;
					}
					break;
				}

				UpdateDisplay();
			}
		}

		void UpdateDisplay()
		{
			textComponent.text = prefix + ProductCount;
		}
	}
}