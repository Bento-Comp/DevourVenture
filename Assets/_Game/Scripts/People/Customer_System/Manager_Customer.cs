using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Customer : UniSingleton.Singleton<Manager_Customer>
{
    public static System.Action<GameObject> OnCustomerSpawn;
    public static System.Action<GameObject, int, int> OnSpawnPositionSent;
    public static System.Action<Customer, int, int> OnExitPositionSent;


    [SerializeField]
    private GridObjectPosition m_spawnGridObjectPosition = null;

    [SerializeField]
    private GridObjectPosition m_exitGridObjectPosition = null;

    [SerializeField]
    private GameObject m_customerPrefab = null;

    [SerializeField]
    private Transform m_customerParent = null;

    [SerializeField]
    private float m_timeBetweenSpawns = 3f;

    [SerializeField]
    private int m_maxCustomerInitialCount = 1;


    private GameObject m_instantiatedCustomer;
    private float m_timer;
    private int m_currentCustomerCount;
    private int m_currentMaxCustomer;


    public GridObjectPosition ExitGridObjectPosition { get => m_exitGridObjectPosition; }


    protected override void OnSingletonEnable()
    {
        base.OnSingletonEnable();
        Customer.OnCustomerLeaving += OnCustomerLeaving;
        Customer.OnCustomerLeaves += OnCustomerLeaves;
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired += OnGlobalUpgradeAquired;
    }


    private void OnDisable()
    {
        Customer.OnCustomerLeaving -= OnCustomerLeaving;
        Customer.OnCustomerLeaves -= OnCustomerLeaves;
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired -= OnGlobalUpgradeAquired;
    }


    private void Start()
    {
        m_currentCustomerCount = 0;
        m_timer = 0f;
        m_currentMaxCustomer = m_maxCustomerInitialCount;
    }


    private void Update()
    {
        TryToSpawnCustomer();
    }


    private void OnGlobalUpgradeAquired(GlobalUpgrade globalUpgrade)
    {
        if (globalUpgrade.m_bonus == Bonus.AdditionalCustomer)
        {
            m_currentMaxCustomer += globalUpgrade.m_count;
        }
    }


    private void TryToSpawnCustomer()
    {
        if (m_currentCustomerCount < m_currentMaxCustomer)
        {
            m_timer += Time.deltaTime;

            if (m_timer > m_timeBetweenSpawns)
            {
                m_timer = 0f;
                SpawnCustomer();
            }
        }
    }


    private void SpawnCustomer()
    {
        m_currentCustomerCount++;

        m_instantiatedCustomer = Instantiate(m_customerPrefab, m_customerParent);

        OnCustomerSpawn?.Invoke(m_instantiatedCustomer);
        OnSpawnPositionSent?.Invoke(m_instantiatedCustomer, m_spawnGridObjectPosition.XPosition, m_spawnGridObjectPosition.YPosition);
    }


    private void OnCustomerLeaving(Customer customerReference)
    {
        OnExitPositionSent?.Invoke(customerReference, m_exitGridObjectPosition.XPosition, m_exitGridObjectPosition.YPosition);
    }

    private void OnCustomerLeaves()
    {
        m_currentCustomerCount--;
    }
}
