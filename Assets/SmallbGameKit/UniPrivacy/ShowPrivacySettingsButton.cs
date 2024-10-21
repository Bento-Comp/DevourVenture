using UnityEngine;
using System.Collections;

using UniUI;

namespace UniPrivacy
{
	[AddComponentMenu("UniPrivacy/ShowPrivacySettingsButton")]
	public class ShowPrivacySettingsButton : MenuButton
	{
		public override void OnClick()
		{
			PrivacyManager.Instance.ShowPrivacySettings();
		}
	}
}