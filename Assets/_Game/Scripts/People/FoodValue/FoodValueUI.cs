using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodValueUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_foodValueUI = null;

    [SerializeField]
    private Employee m_employeeReference = null;

    [SerializeField]
    private TMP_Text m_foodValueText = null;

    public Employee EmployeeReference { get => m_employeeReference; set => m_employeeReference = value; }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        m_employeeReference.OnFoodGenerated -= OnFoodGenerated;
        m_employeeReference.OnCustomerServed -= OnCustomerServed;
        m_employeeReference.OnFoodPutOnTransitionCounter -= OnFoodPutOnTransitionCounter;
    }

    private void Start()
    {
        m_foodValueUI.SetActive(false);
    }

    public void Initialize()
    {
        if (m_employeeReference == null)
            return;

        m_employeeReference.OnFoodGenerated += OnFoodGenerated;
        m_employeeReference.OnCustomerServed += OnCustomerServed;
        m_employeeReference.OnFoodPutOnTransitionCounter += OnFoodPutOnTransitionCounter;
    }

    private void OnFoodGenerated(FoodType foodType)
    {
        m_foodValueUI.SetActive(true);

        IdleNumber gains_IdleNumber = Manager_FoodStats.Instance.GetFoodStats(foodType).EvaluateGain(Manager_Stand.Instance.GetStand(foodType).Level)
            * Manager_MoneyMultiplier.Instance.GetFoodMultiplier(foodType)
            * Manager_MoneyMultiplier.Instance.GetGlobalMoneyMultiplier()
            * Manager_MoneyMultiplier.Instance.GetGlobalMoneyTemporaryMultiplier()
            * Manager_MoneyMultiplier.Instance.GetEquipmentMoneyMultiplier();

        m_foodValueText.text = IdleNumber.FormatIdleNumberText(gains_IdleNumber);
    }


    private void OnCustomerServed()
    {
        m_foodValueUI.SetActive(false);
    }

    private void OnFoodPutOnTransitionCounter()
    {
        m_foodValueUI.SetActive(false);
    }

}
