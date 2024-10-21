using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniEditor
{
	public static class ComponentBuilderUtility
	{
		public static GameObject CopyComponents<ComponentType>(string name, GameObject gameObject)
			where ComponentType : Component
		{
			return CopyComponents(name, gameObject, new Type[]{typeof(ComponentType)});
		}

		public static GameObject CopyComponents(string name, GameObject gameObject, IEnumerable<Type> componentTypesToCopy)
		{
			GameObject copy = GameObject.Instantiate(gameObject) as GameObject;
			copy.name = name;
			DestroyComponents(copy, componentTypesToCopy, true, true);

			return copy;
		}

		public static void DestroyComponents(GameObject gameObject, IEnumerable<Type> componentTypes, bool keepComponentTypes, bool immediateDestruction)
		{
			foreach(Component component in gameObject.GetComponents<Component>())
			{
				if(component is Transform)
					continue;

				bool isTypeInList = false;
				foreach(Type componentTypeToKeep in componentTypes)
				{
					Type componentType = component.GetType();
					if(componentType == componentTypeToKeep || componentType.IsSubclassOf(componentTypeToKeep))
					{
						isTypeInList = true;
						break;
					}
				}

				if((keepComponentTypes && isTypeInList)
				   || (!keepComponentTypes && !isTypeInList))
					continue;

				if(immediateDestruction)
				{
					GameObject.DestroyImmediate(component);
				}
				else
				{
					GameObject.Destroy(component);
				}
			}
		}

		public static TComponentType GetOrAddComponent<TComponentType>(Component a_rComponent) where TComponentType : Component
		{
			if(a_rComponent == null)
			{
				return null;
			}
			else
			{
				return GetOrAddComponent<TComponentType>(a_rComponent.gameObject);
			}
		}
		
		public static TComponentType GetOrAddComponent<TComponentType>(GameObject a_rGameObject) where TComponentType : Component
		{
			if(a_rGameObject == null)
			{
				return null;
			}
			else
			{
				TComponentType rComponent = a_rGameObject.GetComponent<TComponentType>();	
				if(rComponent == null)
				{
					rComponent = a_rGameObject.AddComponent<TComponentType>();
				}
				
				return rComponent;
			}
		}
		
		public static ComponentType BuildAtSamePlace<ComponentType>(Component a_rObjectPlace) where ComponentType : Component
		{
			ComponentType rBuiltComponent = BuildComponent<ComponentType>(typeof(ComponentType).Name);
			
			Transform rBuiltComponentTransform = rBuiltComponent.transform;
			
			rBuiltComponentTransform.parent = a_rObjectPlace.transform.parent;
			rBuiltComponentTransform.localPosition = a_rObjectPlace.transform.localPosition;
			rBuiltComponentTransform.localRotation = a_rObjectPlace.transform.localRotation;
			rBuiltComponentTransform.localScale = a_rObjectPlace.transform.localScale;
			
			return rBuiltComponent;
		}
		
		public static ComponentType BuildComponent<ComponentType>() where ComponentType : Component
		{
			return BuildComponent<ComponentType>(typeof(ComponentType).Name, (Component)null);
		}
		
		public static ComponentType BuildComponent<ComponentType>(string a_rGameObjectName) where ComponentType : Component
		{
			return BuildComponent<ComponentType>(a_rGameObjectName, (Component)null);
		}
		
		public static ComponentType BuildComponent<ComponentType>(GameObject a_rParent) where ComponentType : Component
		{
			return BuildComponent<ComponentType>(typeof(ComponentType).Name, a_rParent);
		}
		
		public static ComponentType BuildComponent<ComponentType>(Component a_rParent) where ComponentType : Component
		{
			return BuildComponent<ComponentType>(typeof(ComponentType).Name, a_rParent.gameObject);
		}
		
		public static ComponentType BuildComponent<ComponentType>(string a_rGameObjectName, GameObject a_rParent) where ComponentType : Component
		{
			return BuildComponent<ComponentType>(a_rGameObjectName, (a_rParent==null)?null:a_rParent.transform);
		}
		
		public static ComponentType BuildComponent<ComponentType>(string a_rGameObjectName, Component a_rParent) where ComponentType : Component
		{
			return BuildComponent(typeof(ComponentType), a_rGameObjectName, a_rParent) as ComponentType;
		}
		
		public static Component BuildComponent(Type a_rComponentType)
		{
			return BuildComponent(a_rComponentType, a_rComponentType.Name, null);
		}
		
		public static Component BuildComponent(Type a_rComponentType, string a_rGameObjectName)
		{
			return BuildComponent(a_rComponentType, a_rGameObjectName, null);
		}
		
		public static Component BuildComponent(Type a_rComponentType, Component a_rParent)
		{
			return BuildComponent(a_rComponentType, a_rComponentType.Name, a_rParent);
		}
	
		public static Component BuildComponent(Type a_rComponentType, string a_rGameObjectName, Component a_rParent)
		{
			GameObject rNewGameObject;
			
			// Create a new game object to contain the component
			rNewGameObject = new GameObject(a_rGameObjectName);
			
			// If the new object have a parent
			if(a_rParent != null)
			{
				// Attach it to the parent
				rNewGameObject.transform.parent = a_rParent.transform;
				rNewGameObject.transform.localPosition = Vector3.zero;
				rNewGameObject.transform.localRotation = Quaternion.identity;
				rNewGameObject.transform.localScale = Vector3.one;
			}
			
			// Try to get the component (Can already be on the game object if ComponentType == Transform)
			Component rComponent = rNewGameObject.GetComponent(a_rComponentType);
			if(rComponent == null)
			{
				// Add a new component to the action object
				return rNewGameObject.AddComponent(a_rComponentType);
			}
			else
			{
				return rComponent;
			}
		}
		
		public static Component BuildComponentIfDoesntExist(Type a_rComponentType, string a_rGameObjectName, Component a_rParent)
		{
			Component rComponentInScene = MonoBehaviour.FindObjectOfType(a_rComponentType) as Component;
			if(rComponentInScene != null)
			{
				return rComponentInScene;
			}
			
			return BuildComponent(a_rComponentType, a_rGameObjectName, a_rParent);
		}
	
		public static ComponentType BuildComponentIfDoesntExist<ComponentType>() where ComponentType : Component
		{
			return BuildComponentIfDoesntExist<ComponentType>(typeof(ComponentType).Name, (Component)null);
		}
		
		public static ComponentType BuildComponentIfDoesntExist<ComponentType>(string a_rGameObjectName) where ComponentType : Component
		{
			return BuildComponentIfDoesntExist<ComponentType>(a_rGameObjectName, (Component)null);
		}
		
		public static ComponentType BuildComponentIfDoesntExist<ComponentType>(GameObject a_rParent) where ComponentType : Component
		{
			return BuildComponentIfDoesntExist<ComponentType>(typeof(ComponentType).Name, a_rParent);
		}
		
		public static ComponentType BuildComponentIfDoesntExist<ComponentType>(Component a_rParent) where ComponentType : Component
		{
			return BuildComponentIfDoesntExist<ComponentType>(typeof(ComponentType).Name, a_rParent.gameObject);
		}
		
		public static ComponentType BuildComponentIfDoesntExist<ComponentType>(string a_rGameObjectName, GameObject a_rParent) where ComponentType : Component
		{
			return BuildComponentIfDoesntExist<ComponentType>(a_rGameObjectName, (a_rParent==null)?null:a_rParent.transform);
		}
		
		public static ComponentType BuildComponentIfDoesntExist<ComponentType>(string a_rGameObjectName, Component a_rParent) where ComponentType : Component
		{
			return BuildComponentIfDoesntExist(typeof(ComponentType), a_rGameObjectName, a_rParent) as ComponentType;
		}
		
		public static Component BuildComponentIfDoesntExist(Type a_rComponentType)
		{
			return BuildComponentIfDoesntExist(a_rComponentType, a_rComponentType.Name, null);
		}
		
		public static Component BuildComponentIfDoesntExist(Type a_rComponentType, string a_rGameObjectName)
		{
			return BuildComponentIfDoesntExist(a_rComponentType, a_rGameObjectName, null);
		}
		
		public static Component BuildComponentIfDoesntExist(Type a_rComponentType, Component a_rParent)
		{
			return BuildComponentIfDoesntExist(a_rComponentType, a_rComponentType.Name, a_rParent);
		}
	}
}
