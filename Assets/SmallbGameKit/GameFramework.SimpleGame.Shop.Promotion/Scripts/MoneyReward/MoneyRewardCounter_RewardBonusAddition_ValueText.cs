using UnityEngine;

using UnityEngine.UI;

using UniMoney;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/MoneyRewardCounter_RewardBonusAddition_ValueText")]
	public class MoneyRewardCounter_RewardBonusAddition_ValueText : MonoBehaviour 
	{
		public Text textComponent;

		public GameObject activationRoot;

		void Awake()
		{
			MoneyRewardManager.onRewardBonusAdditionGameplayChange += OnRewardBonusAdditionGameplayChange;
		}

		void OnDestroy()
		{
			MoneyRewardManager.onRewardBonusAdditionGameplayChange -= OnRewardBonusAdditionGameplayChange;
		}

		void Start()
		{
			OnRewardBonusAdditionGameplayChange();
		}

		void OnRewardBonusAdditionGameplayChange()
		{
			int value = MoneyRewardManager.Instance.RewardBonusAdditionGameplay;
	
			SetText(value.ToString());

			activationRoot.SetActive(value > 0);
		}

		void SetText(string text)
		{
			textComponent.text = text;
		}
	}
}