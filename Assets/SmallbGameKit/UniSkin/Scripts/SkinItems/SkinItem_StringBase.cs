﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_StringBase")]
	public abstract class SkinItem_StringBase : SkinItemBase
	{
		public abstract string GetString(int index = 0, int count = 1);
	}
}
