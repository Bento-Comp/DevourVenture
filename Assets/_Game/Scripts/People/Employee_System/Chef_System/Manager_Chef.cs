using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Chef : UniSingleton.Singleton<Manager_Chef>
{
    public static System.Action<GameObject> OnChefSpawn;
    public static System.Action<GameObject, int, int> OnSpawnPositionSent;
    public static System.Action OnSpeedMultiplierUpdated;


    [SerializeField]
    private GridObjectPosition m_spawnGridObjectPosition = null;

    [SerializeField]
    private GameObject m_chefPrefab = null;

    [SerializeField]
    private Transform m_chefParent = null;

    [SerializeField]
    private GameObject m_mainCharacterGameobject = null;

    [SerializeField]
    private float m_movementSpeedBoostPercentage = 0.2f;


    private GameObject m_instantiatedChef;
    private float m_chefSpeedMultiplier;
    private float m_chefInitialSpeedMultiplier = 1f;
    private int m_boostCount = 0;

    public float ChefSpeedMultiplier { get => m_chefSpeedMultiplier; }


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
            SpawnChef();

            m_mainCharacterGameobject.SetActive(false);
        }
        else
        {
            m_mainCharacterGameobject.SetActive(true);
        }


        m_chefSpeedMultiplier = m_chefInitialSpeedMultiplier;

        OnSpeedMultiplierUpdated?.Invoke();
    }


    private void OnGlobalUpgradeAquired(GlobalUpgrade globalUpgrade)
    {
        if (globalUpgrade.m_bonus == Bonus.ChefsWalkFaster)
        {
            m_boostCount++;
            m_chefSpeedMultiplier = m_chefInitialSpeedMultiplier + m_movementSpeedBoostPercentage * m_boostCount;

            OnSpeedMultiplierUpdated?.Invoke();
        }

        if (globalUpgrade.m_bonus == Bonus.AdditionalChef)
        {
            for (int i = 0; i < globalUpgrade.m_count; i++)
                SpawnChef();
        }
    }


    private void SpawnChef()
    {
        m_instantiatedChef = Instantiate(m_chefPrefab, m_chefParent);

        OnChefSpawn?.Invoke(m_instantiatedChef);
        OnSpawnPositionSent?.Invoke(m_instantiatedChef, m_spawnGridObjectPosition.XPosition, m_spawnGridObjectPosition.YPosition);
    }

}
