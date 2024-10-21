using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame.MoneyUIGiverInternal
{
	[AddComponentMenu("GameFramework/SimpleGame/GivenItem_Target")]
	public class GivenItem_Target : MonoBehaviour
	{
		public Transform target;

		public Vector2 TargetPosition => target.position;
	}
}