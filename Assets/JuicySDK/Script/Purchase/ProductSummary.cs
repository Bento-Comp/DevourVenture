using UnityEngine;
using System.Collections;

namespace Juicy
{
	[System.Serializable]
	public class ProductSummary
	{
		public string productId;

		public ProductType productType;

		public string transactionId;

		public string localizedPriceString;

		public decimal localizedPrice;

		public string receipt;

		public string isoCurrencyCode;

		public ProductSummary()
		{
		}

		public ProductSummary(string productId)
		{
			this.productId = productId;
		}
	}
}
