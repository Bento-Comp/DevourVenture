using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Money_UI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_moneyText = null;


    private void OnEnable()
    {
        Manager_Money.OnUpdateMoney += OnUpdateMoney;
    }

    private void OnDisable()
    {
        Manager_Money.OnUpdateMoney -= OnUpdateMoney;
    }


    private void OnUpdateMoney()
    {
        m_moneyText.text = FormatMoneyText(Manager_Money.Instance.Money);
    }


    private string FormatMoneyText(IdleNumber amount_Idlenumber)
    {
        return IdleNumber.FormatIdleNumberText(amount_Idlenumber);
    }
}
