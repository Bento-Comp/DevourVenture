using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[Serializable]
	public class ActivationGroup
	{
		public ActivationControllerBase[] activationControllers = new ActivationControllerBase[0];

		public GameObject[] gameObjects = new GameObject[0];
		public MonoBehaviour[] components = new MonoBehaviour[0];

		public GameObject[] gameObjectsToDeactivate = new GameObject[0];
		public MonoBehaviour[] componentsToDeactivate = new MonoBehaviour[0];

		public int[] childrenIndices = new int[0];

		public void Activate(bool activate, List<Activator> childrenActivators, ActivationGroup selectedActivationGroup = null, bool setFirstState = false)
		{
			foreach(ActivationControllerBase activationController in activationControllers)
			{
				if(activationController == null)
					continue;

				if(selectedActivationGroup != null && activate == false && selectedActivationGroup.Contains(activationController))
				{
					continue;
				}

				activationController.Activate(activate, setFirstState);
			}

			foreach(MonoBehaviour component in components)
			{
				if(component == null)
					continue;

				if(selectedActivationGroup != null && activate == false && selectedActivationGroup.Contains(component))
				{
					continue;
				}

				component.enabled = activate;
			}
			
			foreach(GameObject gameObject in gameObjects)
			{
				if(gameObject == null)
					continue;

				if(selectedActivationGroup != null && activate == false && selectedActivationGroup.Contains(gameObject))
				{
					continue;
				}

				if(gameObject.activeSelf != activate)
					gameObject.SetActive(activate);
			}

			if(activate)
			{
				for(int i = 0; i < childrenActivators.Count; ++i)
				{
					Activator childrenActivator = childrenActivators[i];

					if(childrenActivator == null)
						continue;

					int childrenIndex = 0;
					if(i < childrenIndices.Length)
					{
						childrenIndex = childrenIndices[i];
					}

					if(setFirstState)
					{
						childrenActivator.SetFirstActiveState(childrenIndex);
					}
					else
					{
						childrenActivator.SelectedIndex = childrenIndex;
					}
				}
			}

			// Deactivation
			activate = !activate;
			foreach(MonoBehaviour component in componentsToDeactivate)
			{
				if(component == null)
					continue;
				
				if(selectedActivationGroup != null && activate == false && selectedActivationGroup.Contains(component))
				{
					continue;
				}
				
				component.enabled = activate;
			}
			
			foreach(GameObject gameObject in gameObjectsToDeactivate)
			{
				if(gameObject == null)
					continue;
				
				if(selectedActivationGroup != null && activate == false && selectedActivationGroup.Contains(gameObject))
				{
					continue;
				}
				
				gameObject.SetActive(activate);
			}
		}

		public bool Contains(ActivationControllerBase testedActivationController)
		{
			foreach(ActivationControllerBase activationController in activationControllers)
			{
				if(testedActivationController == activationController)
					return true;
			}

			return false;
		}

		public bool Contains(GameObject testedGameObject)
		{
			foreach(GameObject gameObject in gameObjects)
			{
				if(testedGameObject == gameObject)
					return true;
			}
			
			return false;
		}

		public bool Contains(MonoBehaviour testedComponent)
		{
			foreach(MonoBehaviour component in components)
			{
				if(testedComponent == component)
					return true;
			}

			return false;
		}
	}
}