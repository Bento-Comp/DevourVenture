using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UniMoney
{
	[CustomEditor(typeof(MoneyManager))]
	[CanEditMultipleObjects()]
	public class MoneyManager_Inspector : Editor 
	{
		int operationValue = 1;
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			MoneyManager manager = target as MoneyManager;

			operationValue = EditorGUILayout.IntField("Test Money Operation Value", operationValue);
			foreach(MoneyManager.StartMoneyValue startMoneyValue in manager.startMoneyValues)
			{
				string moneyName = startMoneyValue.moneyName;

				GUILayout.Label(moneyName + " = " + manager.GetMoney(moneyName));

				if(GUILayout.Button("Add " + operationValue + " " + moneyName))
				{
					manager.AddMoney(moneyName, operationValue);
				}

				if(GUILayout.Button("Try Use " + operationValue + " " + moneyName))
				{
					manager.TryUseMoney(moneyName, operationValue);
				}

				if(GUILayout.Button("Clear " + moneyName))
				{
					manager.SetMoney(moneyName, 0);
				}
			}
		}
	}
}