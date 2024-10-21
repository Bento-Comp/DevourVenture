using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniPrivacy
{
	[DefaultExecutionOrder(1)]
	[AddComponentMenu("UniPrivacySettings/PrivacyCaller")]
	public abstract class PrivacyCaller : MonoBehaviour
	{
		public abstract void ShowPrivacySettings();

		void Awake()
		{
			PrivacyManager.Instance.SetCaller(this);
		}
	}
}