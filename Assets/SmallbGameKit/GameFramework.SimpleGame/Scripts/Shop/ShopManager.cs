using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniActivation;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/ShopManager")]
	public class ShopManager : MonoBehaviour
	{
		Dictionary<string, ShopOpenController> shopOpenControllers = new Dictionary<string, ShopOpenController>();
		
		static ShopManager instance;

		public static ShopManager Instance
		{
			get
			{
				return instance;
			}
		}

		public void RegisterShopOpenController(string shopName, ShopOpenController controller)
		{
			if(shopOpenControllers.ContainsKey(shopName))
			{
				shopOpenControllers.Remove(shopName);
			}

			shopOpenControllers.Add(shopName, controller);
		}

		public void UnregisterShopOpenController(string shopName)
		{
			shopOpenControllers.Remove(shopName);
		}

		public void OpenShop(string shopName, bool open)
		{
			Debug.Log("OpenShop : " + shopName + " : " + open);

			ShopOpenController controller;
			if(shopOpenControllers.TryGetValue(shopName, out controller))
			{
				controller.OpenShop(open);
			}
		}

		public void OpenShop(string shopName)
		{
			OpenShop(shopName, true);
		}

		public void CloseShop(string shopName)
		{
			OpenShop(shopName, false);
		}

		void Awake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}
		
		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}
	}
}