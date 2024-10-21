using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniSkin;
using UniHapticFeedback;

namespace GameFramework.SimpleGame
{
	public static class CharacterModelsUtilities
	{
		public static string mainColorSkinName = "MainColor";
		public static string secondaryColorSkinName = "SecondaryColor";

		public static void SetLayer_AllChildren(GameObject gameObject, string layerName)
		{
			int layerIndex = LayerMask.NameToLayer(layerName);
			foreach(Transform childTransform in gameObject.GetComponentsInChildren<Transform>())
			{
				childTransform.gameObject.layer = layerIndex;
			}
		}

		public static void SetSkinApplicator(List<Renderer> renderers, string colorName, SkinUserBase skinUser = null)
		{
			if(renderers == null)
				return;

			foreach(Renderer rendererComponent in renderers)
			{
				bool addedMaterialInstance;
				MaterialInstance materialInstance = AddOrGetComponent<MaterialInstance>(rendererComponent, out addedMaterialInstance);

				materialInstance.rendererComponent = rendererComponent;

				if(addedMaterialInstance)
				{
					materialInstance.sharedMaterial = rendererComponent.sharedMaterial;
					materialInstance.color = rendererComponent.sharedMaterial.color;
				}

				SkinItemApplicator_MaterialInstance_Color applicator = AddOrGetComponent<SkinItemApplicator_MaterialInstance_Color>(materialInstance);

				applicator.materialInstance = materialInstance;

				applicator.skinItemName = colorName;

				applicator.skinUser = skinUser;
			}
		}

		public static TComponent AddOrGetComponent<TComponent>(GameObject gameObject) where TComponent : Component
		{
			return AddOrGetComponent<TComponent>(gameObject.transform);
		}

		public static TComponent AddOrGetComponent<TComponent>(Component otherComponent) where TComponent : Component
		{
			bool added;
			return AddOrGetComponent<TComponent>(otherComponent, out added);
		}

		public static TComponent AddOrGetComponent<TComponent>(Component otherComponent, out bool added) where TComponent : Component
		{
			added = false;

			TComponent component = otherComponent.GetComponent<TComponent>();
			if(component == null)
			{
				#if UNITY_EDITOR
				if(Application.isPlaying == false)
				{
					component = UnityEditor.Undo.AddComponent<TComponent>(otherComponent.gameObject);
					CharacterModelsUtilities.SetDirty(component);
				}
				else
				#endif
				{
					component = otherComponent.gameObject.AddComponent<TComponent>();
				}

				added = true;
			}

			return component;
		}

		public static TInstance InstantiatePrefab<TInstance>(TInstance prefab, Transform parent) where TInstance : Object
		{
			TInstance instance;

			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				instance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent) as TInstance;
			}
			else
			#endif
			{
				instance = GameObject.Instantiate<TInstance>(prefab, parent, false);
			}

			return instance;
		}

		public static void SetDirty(Object target)
		{
			#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(target);
			#endif
		}
	}
}