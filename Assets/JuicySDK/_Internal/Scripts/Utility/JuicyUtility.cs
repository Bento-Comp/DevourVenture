using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuicyInternal
{
	public static class JuicyUtility
	{
		public static ComponentType CreateSubManager<ComponentType>(Transform parentTransform, ComponentType componentPrefab) where ComponentType : MonoBehaviour
		{
			ComponentType manager = MonoBehaviour.Instantiate(componentPrefab, parentTransform) as ComponentType;

			manager.name = typeof(ComponentType).Name;

			return manager;
		}

		public static ComponentType CreateSubManager<ComponentType>(Transform parentTransform) where ComponentType : MonoBehaviour
		{
			GameObject managerGameObject = new GameObject(typeof(ComponentType).Name);

			managerGameObject.transform.SetParent(parentTransform, false);

			ComponentType manager = managerGameObject.AddComponent<ComponentType>();

			return manager;
		}

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}