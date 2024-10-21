using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniActivation;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/ShopOpenController")]
	public class ShopOpenController : MonoBehaviour
	{
		public string shopName;

		public Activator shopScreenActivator;

		public void OpenShop(bool open)
		{
			if(open && Game.Instance.IsGameStarted)
				return;
			
			shopScreenActivator.SelectedIndex = open ? 1 : 0;
		}

		void OnEnable()
		{
			OpenShop(false);
		}

		void Awake()
		{
			OpenShop(false);
		}

		void Start()
		{
			ShopManager.Instance.RegisterShopOpenController(shopName, this);
		}
	}
}