using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using GameFramework.SimpleGame;

namespace GameFramework.SimpleGame.Ads
{
	[AddComponentMenu("GameFramework/SimpleGame/Money/AddMoneyOnGameOverConfirmed")]
	public class AddMoneyOnGameOverConfirmed : MonoBehaviour
	{
		public string moneyName;
		
		public int moneyToGive = 1;

		void Awake()
		{
			ContinueManager.onGameOverConfirmed += OnGameOverConfirmed;
		}

		void OnDestroy()
		{
			ContinueManager.onGameOverConfirmed -= OnGameOverConfirmed;
		}
		
		void OnGameOverConfirmed()
		{
			UniMoney.MoneyManager.Instance.AddMoney(moneyName, moneyToGive);
		}
	}
}