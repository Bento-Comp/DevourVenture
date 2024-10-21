using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_Sprite_ByPlatform")]
	public class SkinItem_Sprite_ByPlatform : SkinItem_SpriteBase
	{
		[System.Serializable]
		public class SpriteByPlatform
		{
			public Sprite sprite;

			public List<RuntimePlatform> platforms;

			public bool Contains(RuntimePlatform platform)
			{
				return platforms.Contains(platform);
			}
		}

		public Sprite defaultSprite;

		public List<SpriteByPlatform> sprites;

		Sprite sprite;

		public override Sprite GetSprite(int index = 0, int count = 1)
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				sprite = null;
			#endif

			if(sprite == null)
			{
				bool found = false;
				foreach(SpriteByPlatform selector in sprites)
				{
					if(selector.Contains(Application.platform))
					{
						found = true;
						sprite = selector.sprite;
						break;
					}
				}
				if(found == false)
				{
					sprite = defaultSprite;
				}
			}

			return sprite;
		}
	}
}
