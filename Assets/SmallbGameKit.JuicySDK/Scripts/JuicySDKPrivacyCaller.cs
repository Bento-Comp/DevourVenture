using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using GameFramework;

using UniPrivacy;

using Juicy;

namespace SmallbGameKit
{
	[AddComponentMenu("SmallbGameKit/JuicySDK/JuicySDKPrivacyCaller")]
	public class JuicySDKPrivacyCaller : PrivacyCaller
	{
		public override void ShowPrivacySettings()
		{
			JuicySDK.ShowPrivacySettings();
		}
	}
}
