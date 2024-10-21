using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniSkin;

namespace GameFramework.SimpleGame
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GameFramework/SimpleGame/SkinItem_CharacterModel")]
	public class SkinItem_CharacterModel : SkinItemBase
	{
		[SerializeField]
		CharacterModel model = null;

		public CharacterModel GetModel()
		{
			return model;
		}
	}
}