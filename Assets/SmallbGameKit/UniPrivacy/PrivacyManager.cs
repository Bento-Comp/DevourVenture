using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniPrivacy
{
	[AddComponentMenu("UniPrivacySettings/PrivacyManager")]
	public class PrivacyManager : MonoBehaviour
	{
		PrivacyCaller caller;

		PrivacyCaller Caller
		{
			get
			{
				return caller;
			}
		}

		public void ShowPrivacySettings()
		{
			caller.ShowPrivacySettings();
		}

		public void SetCaller(PrivacyCaller caller)
		{
			this.caller = caller;
		}

		static PrivacyManager instance;
		
		public static PrivacyManager Instance
		{
			get
			{
				return instance;
			}
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