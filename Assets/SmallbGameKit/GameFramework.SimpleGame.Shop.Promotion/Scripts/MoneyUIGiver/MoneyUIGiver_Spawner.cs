using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework.SimpleGame.MoneyUIGiverInternal;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(-1)]
	[AddComponentMenu("GameFramework/SimpleGame/MoneyUIGiver_Spawner")]
	public class MoneyUIGiver_Spawner : MonoBehaviour
	{
		class SpawnParameters
		{
			public int spawnCount;
			public int valueBySpawn;
			public int lastSpawnValue;

			public string moneyName;
		}

		public MoneyUIGiver giver;

		public int maxSpawnCount = 10;
		public int minValueBySpawn = 10;

		public Transform givenItemsRoot;
		public Transform startSpawnPosition;
		public Transform targetTransform;
		public float startSpawnRadius = 1.0f; 

		public GivenItem givenItemPrefab;

		HashSet<GivenItem> givenItemsInProgress = new HashSet<GivenItem>();

		void OnEnable()
		{
			giver.onGiveMoney += OnGiveMoney;
		}

		void OnDisable()
		{
			giver.onGiveMoney -= OnGiveMoney;
		}

		void OnGiveMoney(int moneyValue, string moneyName)
		{
			SpawnParameters spawnParameters = ComputeSpawnParameters(moneyValue);
			Spawn(spawnParameters);
		}

		SpawnParameters ComputeSpawnParameters(int moneyValue)
		{
			SpawnParameters spawnParameters = new SpawnParameters();

			// Spawn Count
			int wantedSpawnCount;
			if(moneyValue < 0)
			{
				wantedSpawnCount = 0;
			}
			else
			{
				wantedSpawnCount = moneyValue/minValueBySpawn;

				if(moneyValue % minValueBySpawn > 0)
					++wantedSpawnCount;
			}
			int spawnCount = Mathf.Min(wantedSpawnCount, maxSpawnCount);

			// Value By Spawn
			int valueBySpawn = (spawnCount > 0) ? moneyValue/spawnCount : 0;
			int lastSpawnValue = (spawnCount > 0) ? (valueBySpawn + moneyValue % spawnCount) : 0;

			// Fill Parameters
			spawnParameters.spawnCount = spawnCount;
			spawnParameters.valueBySpawn = valueBySpawn;
			spawnParameters.lastSpawnValue = lastSpawnValue;
			spawnParameters.moneyName = MoneyRewardManager.Instance.CurrentMoney;

			return spawnParameters;
		}

		void Spawn(SpawnParameters spawnParameters)
		{
			for(int i = 0; i < spawnParameters.spawnCount; ++i)
			{
				int spawnValue = (i == spawnParameters.spawnCount - 1)?
					spawnParameters.lastSpawnValue
					: spawnParameters.valueBySpawn;

				Spawn(spawnValue, spawnParameters.moneyName);
			}
		}

		void Spawn(int moneyValue, string moneyName)
		{
			GivenItem givenItem = Instantiate(givenItemPrefab, givenItemsRoot, false);

			givenItem.moneyToGive.moneyName = moneyName;
			givenItem.moneyToGive.moneyValue = moneyValue;

			Vector2 randomPositionInUnitCircle = Random.insideUnitCircle;
			givenItem.body.Position = (Vector2)startSpawnPosition.position + randomPositionInUnitCircle * startSpawnRadius;

			givenItem.target.target = targetTransform;
			givenItem.spawnCenter.distanceFromCenterInPercent = randomPositionInUnitCircle;

			givenItem.giver.onGived += OnGived;

			givenItemsInProgress.Add(givenItem);
		}

		void OnGived(GivenItem givenItem)
		{
			if(givenItemsInProgress.Remove(givenItem) == false)
				return;

			givenItem.giver.onGived -= OnGived;

			if(givenItemsInProgress.Count <= 0)
			{
				giver.NotifyGiveMoneyEnd();
			}
		}

		void OnDrawGizmosSelected()
		{
			if(startSpawnPosition != null)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(startSpawnPosition.position, startSpawnRadius);
			}

			if(targetTransform != null)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(targetTransform.position, givenItemPrefab.goToTarget.targetReachedRadius);
			}
		}
	}
}