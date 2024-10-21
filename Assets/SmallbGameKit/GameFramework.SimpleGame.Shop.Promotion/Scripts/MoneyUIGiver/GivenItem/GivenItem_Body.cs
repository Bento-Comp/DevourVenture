using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame.MoneyUIGiverInternal
{
	[AddComponentMenu("GameFramework/SimpleGame/GivenItem_Body")]
	public class GivenItem_Body : MonoBehaviour
	{
		public Transform controlledTransform;

		public Vector2 Position
		{
			get => controlledTransform.position;
			set
			{
				Vector3 position = controlledTransform.position;
				position.x = value.x;
				position.y = value.y;
				controlledTransform.position = position;
			}
		}
	}
}