using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniActivation
{
	[ExecuteInEditMode()]
	[DefaultExecutionOrder(-31998)]
	[AddComponentMenu("UniActivation/NamedActivation")]
	public class NamedActivation : MonoBehaviour
	{
		[SerializeField]
		NamedActivationsRegister localTo;

		string registeredActivationName;

		public string ActivationName
		{
			get
			{
				return name;
			}
		}

		NamedActivationsRegister register;

		public static string GetActivationName(string name, GameObject localTo)
		{
			if(localTo != null)
			{
				return localTo.GetInstanceID() + name;
			}
			else
			{
				return name;
			}
		}

		void OnEnable()
		{
			TryToRegister();
			#if UNITY_EDITOR
			AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
			#endif
		}

		void OnDisable()
		{
			TryToUnregister();
			#if UNITY_EDITOR
			AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
			#endif
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			string currentActivationName = ActivationName;
			if(currentActivationName != registeredActivationName || assemblyReloaded
				|| register != localTo)
			{
				UpdateRegister();
			}

			assemblyReloaded = false;
		}

		bool assemblyReloaded;
		void OnAfterAssemblyReload()
		{
			assemblyReloaded = true;
		}

		void UpdateRegister()
		{
			TryToUnregister();
			TryToRegister();
		}
		#endif

		NamedActivationsRegister RetrieveRegister()
		{
			if(localTo != null)
				return localTo;

			return NamedActivationsManager.Instance;
		}

		void TryToRegister()
		{
			register = RetrieveRegister();

			if(register == null)
				return;

			registeredActivationName = ActivationName;

			if(registeredActivationName == null)
				return;

			register.Register(this, registeredActivationName);
		}

		void TryToUnregister()
		{
			if(register == null)
			{
				registeredActivationName = null;
				return;
			}

			if(registeredActivationName == null)
			{
				register = null;
				return;
			}

			register.Unregister(this, registeredActivationName);

			registeredActivationName = null;
			register = null;
		}
	}
}