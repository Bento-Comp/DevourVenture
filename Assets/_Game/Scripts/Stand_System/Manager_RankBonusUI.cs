using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)] //Must execute after Manager_GlobalUpgrade script because the pop up UIs must not appear at start
public class Manager_RankBonusUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_additionnalMachineUI = null;

    [SerializeField]
    private GameObject m_profitMultiplierUI = null;


    private void OnEnable()
    {
        Stand.OnIncreaseGainMultipler += OnIncreaseGainMultipler;
        Stand.OnMachineUnlocked += OnMachineUnlocked;
    }

    private void OnDisable()
    {
        Stand.OnIncreaseGainMultipler -= OnIncreaseGainMultipler;
        Stand.OnMachineUnlocked -= OnMachineUnlocked;
    }


    private void Start()
    {
        m_additionnalMachineUI.SetActive(false);
        m_profitMultiplierUI.SetActive(false);
    }



    private void OnMachineUnlocked()
    {
        m_additionnalMachineUI.SetActive(false);
        m_additionnalMachineUI.SetActive(true);
    }

    private void OnIncreaseGainMultipler(FoodType foodType, float additionalMultiplier)
    {
        m_profitMultiplierUI.SetActive(false);
        m_profitMultiplierUI.SetActive(true);
    }
}
