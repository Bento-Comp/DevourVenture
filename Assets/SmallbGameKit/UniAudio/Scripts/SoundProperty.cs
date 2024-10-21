using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniAudio
{
	[Serializable]
	public class SoundProperty
	{
		public enum EPropertyType
		{
			Asset,
			ByName
		}

		public EPropertyType type;

		public SoundAsset soundAsset;

		public string soundName;

		public bool Valid
		{
			get
			{
				switch(type)
				{
					case EPropertyType.Asset:
					{
						return soundAsset != null;
					}
				}

				return true;
			}
		}

		public override string ToString()
		{
			switch(type)
			{
				case EPropertyType.Asset:
				{
					return "[SoundProperty] Type : " + type + " Sound Asset : " + soundAsset;
				}

				default:
				case EPropertyType.ByName:
				{
					return "[SoundProperty] Type : " + type + " Sound Name : " + soundName;
				}
			}
		}
	}
}
