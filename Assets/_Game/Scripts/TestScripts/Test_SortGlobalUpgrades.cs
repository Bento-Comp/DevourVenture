using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SortGlobalUpgrades : MonoBehaviour
{
    public List<PurchasableGlobalUpgrade> m_list = null;

    public List<PurchasableGlobalUpgrade> m_listToFillByLoading = null;


    public static string path = "Assets/_Game/ScriptableObjects/GlobalUpgradesData/Level 4";


    private void Start()
    {
        DisplayList();
        m_list.Sort();
        Debug.Log("=================================");
        DisplayList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            LoadData();
    }

    private void DisplayList()
    {
        for (int i = 0; i < m_list.Count; i++)
        {
            Debug.Log(m_list[i].m_globalUpgradeData.m_globalUpgrade.m_cost_IdleNumber.m_value + " * exp(" + m_list[i].m_globalUpgradeData.m_globalUpgrade.m_cost_IdleNumber.m_exp + ")");
        }
    }


    private void LoadData()
    {
        Object[] globalUpgradesSO_Objects = Resources.LoadAll("GlobalUpgradesData_test/Level 1", typeof(GlobalUpgrade_ScriptableObject));

        GlobalUpgrade_ScriptableObject[] globalUpgrade_SOArray = new GlobalUpgrade_ScriptableObject[globalUpgradesSO_Objects.Length];
        globalUpgradesSO_Objects.CopyTo(globalUpgrade_SOArray, 0);


        m_listToFillByLoading = new List<PurchasableGlobalUpgrade>();

        for (int i = 0; i < globalUpgrade_SOArray.Length; i++)
        {
            PurchasableGlobalUpgrade purchasableGlobalUpgrade = new PurchasableGlobalUpgrade();
            purchasableGlobalUpgrade.m_globalUpgradeData = globalUpgrade_SOArray[i];

            m_listToFillByLoading.Add(purchasableGlobalUpgrade);
        }
    }

}
