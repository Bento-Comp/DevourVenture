using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame.MoneyUIGiverInternal
{
	[AddComponentMenu("GameFramework/SimpleGame/GivenItem_Giver")]
	public class GivenItem_Giver : MonoBehaviour
	{
		public System.Action<GivenItem> onGived;

		public GivenItem givenItem;

		public void Give()
		{
			UniMoney.MoneyManager.Instance.AddMoney(givenItem.moneyToGive.moneyName,
				givenItem.moneyToGive.moneyValue);

			onGived?.Invoke(givenItem);

			UniHapticFeedback.HapticFeedbackManager.TriggerHapticFeedback(UniHapticFeedback.EHapticFeedbackType.Light);
		}
	}
}