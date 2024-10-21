using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniSkin;
using UniHapticFeedback;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/CharacterModel")]
	public class CharacterModel : MonoBehaviour
	{
		public SkinUserSimple modelSkinUser;

		public Animator animator;
	}
}