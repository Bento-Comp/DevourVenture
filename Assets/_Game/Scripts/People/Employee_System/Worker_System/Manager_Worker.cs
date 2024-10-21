using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Worker : UniSingleton.Singleton<Manager_Worker>
{
    public static System.Action<GameObject> OnWorkerSpawn;
    public static System.Action<GameObject, int, int> OnSpawnPositionSent;
    public static System.Action OnSpeedMultiplierUpdated;


    [SerializeField]
    private GridObjectPosition m_spawnGridObjectPosition = null;

    [SerializeField]
    private GameObject m_workerPrefab = null;

    [SerializeField]
    private Transform m_workerParent = null;

    [SerializeField]
    private GameObject m_mainCharacterGameobject = null;

    [SerializeField]
    private float m_movementSpeedBoostPercentage = 0.2f;

    private GameObject m_instantiatedWorker;
    private float m_workerSpeedMultiplier;
    private float m_workerInitialSpeedMultiplier = 1f;
    private int m_boostCount = 0;

    public float WorkerSpeedMultiplier { get => m_workerSpeedMultiplier; }


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
        if (!PlayerPrefs.HasKey(MainCharacterUnlocker.m_isMainCharacterPlayerPrefKey) || PlayerPrefs.GetInt(MainCharacterUnlocker.m_isMainCharacterPlayerPrefKey) == 0)
        {
            SpawnWorker();

            m_mainCharacterGameobject.SetActive(false);
        }
        else
        {
            m_mainCharacterGameobject.SetActive(true);
        }


        m_workerSpeedMultiplier = m_workerInitialSpeedMultiplier;
        OnSpeedMultiplierUpdated?.Invoke();
    }


    private void OnGlobalUpgradeAquired(GlobalUpgrade globalUpgrade)
    {
        if (globalUpgrade.m_bonus == Bonus.WorkersWalkFaster)
        {
            m_boostCount++;
            m_workerSpeedMultiplier = m_workerInitialSpeedMultiplier + m_movementSpeedBoostPercentage * m_boostCount;

            OnSpeedMultiplierUpdated?.Invoke();
        }

        if (globalUpgrade.m_bonus == Bonus.AdditionalWorker)
        {
            for (int i = 0; i < globalUpgrade.m_count; i++)
                SpawnWorker();
        }
    }


    private void SpawnWorker()
    {
        m_instantiatedWorker = Instantiate(m_workerPrefab, m_workerParent);

        OnWorkerSpawn?.Invoke(m_instantiatedWorker);
        OnSpawnPositionSent?.Invoke(m_instantiatedWorker, m_spawnGridObjectPosition.XPosition, m_spawnGridObjectPosition.YPosition);
    }
}
