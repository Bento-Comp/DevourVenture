using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame.MoneyUIGiverInternal
{
	[AddComponentMenu("GameFramework/SimpleGame/GivenItem")]
	public class GivenItem : MonoBehaviour
	{
		public GivenItem_MoneyToGive moneyToGive;
		public GivenItem_Giver giver;
		public GivenItem_Body body;
		public GivenItem_Target target;
		public GivenItem_GoToTarget goToTarget;
		public GivenItem_SpawnCenter spawnCenter;
		
		bool dead;

		public void Kill()
		{
			if(dead)
				return;

			dead = true;

			Destroy(gameObject);
		}
	}
}