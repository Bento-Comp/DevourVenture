using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UniSkin;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/DestroyUnlockClickFxOnDisable")]
	public class DestroyUnlockClickFxOnDisable : MonoBehaviour
	{
		void OnDisable()
		{
			Destroy(gameObject);
		}
	}
}