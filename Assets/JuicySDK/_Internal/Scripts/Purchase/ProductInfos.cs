using System;
using System.Collections.Generic;
using UnityEngine;

using Juicy;

namespace JuicyInternal
{
	[System.Serializable]
	public class ProductInfos
	{
		public string productId;
		public ProductType type;

        public ProductInfos (string productId, ProductType type)
        {
            this.productId = productId;
            this.type = type;
        }
	}
}
