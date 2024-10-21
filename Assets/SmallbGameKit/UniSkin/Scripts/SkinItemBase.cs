using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemBase")]
	public class SkinItemBase : MonoBehaviour
	{
		public string skinItemName;

		public List<string> skinItemAliases;
	}
}
