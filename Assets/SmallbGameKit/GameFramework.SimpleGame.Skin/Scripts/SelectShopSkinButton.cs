using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UniSkin;

namespace GameFramework.SimpleGame.Skin
{
	[AddComponentMenu("GameFramework/SimpleGame/SelectShopSkinButton")]
	public class SelectShopSkinButton : MonoBehaviour
	{
		public string shopName;

		public SkinSelector skinSelector;

		Button button;

		void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(OnClick); 
		}

		void OnDestroy()
		{
			if(button != null)
				button.onClick.RemoveListener(OnClick);
		}

		void OnClick()
		{
			if(skinSelector.SkinSelected)
			{
				ShopManager.Instance.CloseShop(shopName);
			}
			else
			{
				skinSelector.SelectSkin();
			}
		}
	}
}