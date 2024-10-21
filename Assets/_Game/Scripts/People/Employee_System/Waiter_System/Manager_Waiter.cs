using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Waiter : UniSingleton.Singleton<Manager_Waiter>
{
    public static System.Action<GameObject> OnWaiterSpawn;
    public static System.Action<GameObject, int, int> OnSpawnPositionSent;


    [SerializeField]
    private GridObjectPosition m_spawnGridObjectPosition = null;

    [SerializeField]
    private GameObject m_waiterPrefab = null;

    [SerializeField]
    private Transform m_waiterParent = null;

    [SerializeField]
    private float m_movementSpeedBoostPercentage = 0.2f;

    private GameObject m_instantiatedWaiter;
    private float m_waiterSpeedMultiplier;
    private float m_waiterInitialSpeedMultiplier = 1f;
    private int m_boostCount = 0;

    public float WaiterSpeedMultiplier { get => m_waiterSpeedMultiplier; }


    protected override void OnSingletonEnable()
    {
        base.OnSingletonEnable();
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired += OnGlobalUpgradeAquired;
    }

    private void OnDisable()
    {
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired -= OnGlobalUpgradeAquired;
    }

    private void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        m_waiterSpeedMultiplier = m_waiterInitialSpeedMultiplier;
        SpawnWaiter();
    }


    private void OnGlobalUpgradeAquired(GlobalUpgrade globalUpgrade)
    {
        if (globalUpgrade.m_bonus == Bonus.CashiersWalkFaster)
        {
            m_boostCount++;
            m_waiterSpeedMultiplier = m_waiterInitialSpeedMultiplier + m_movementSpeedBoostPercentage * m_boostCount;
        }

        if (globalUpgrade.m_bonus == Bonus.AdditionalCashier)
        {
            for (int i = 0; i < globalUpgrade.m_count; i++)
                SpawnWaiter();
        }
    }


    private void SpawnWaiter()
    {
        m_instantiatedWaiter = Instantiate(m_waiterPrefab, m_waiterParent);

        OnWaiterSpawn?.Invoke(m_instantiatedWaiter);
        OnSpawnPositionSent?.Invoke(m_instantiatedWaiter, m_spawnGridObjectPosition.XPosition, m_spawnGridObjectPosition.YPosition);
    }
}
