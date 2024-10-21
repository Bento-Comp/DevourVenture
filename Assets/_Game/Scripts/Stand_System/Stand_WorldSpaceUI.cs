using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class Stand_WorldSpaceUI : MonoBehaviour
{
    [SerializeField]
    private Stand m_stand = null;

    [SerializeField]
    private GameObject m_upgradeArrow = null;



    private void OnEnable()
    {
        Manager_Money.OnUpdateMoney += OnUpdateMoney;
    }

    private void OnDisable()
    {
        Manager_Money.OnUpdateMoney -= OnUpdateMoney;
    }


    private void Start()
    {
        UpdateArrowState();
    }

    private void OnUpdateMoney()
    {
        UpdateArrowState();
    }

    private void UpdateArrowState()
    {
        if (Manager_Money.Instance.HasEnoughMoney(m_stand.UpgradeCost_IdleNumber) && m_stand.CurrentFoodStats != null && m_stand.Level < m_stand.CurrentFoodStats.m_maxLevel)
            m_upgradeArrow.SetActive(true);
        else
            m_upgradeArrow.SetActive(false);
    }
}
