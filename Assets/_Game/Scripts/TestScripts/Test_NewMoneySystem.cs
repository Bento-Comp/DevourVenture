using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_NewMoneySystem : MonoBehaviour
{

    public IdleNumber m_number_1 = null;

    public IdleNumber m_number_2 = null;

    public float m_multiplyFactor = 2;

    public float m_divideFactor = 2;

    private void Start()
    {
        Debug.Log("IDLE numbers operations ==========================");
        Debug.Log("Add");
        IdleNumber.PrintIdleNumber(m_number_1 + m_number_2);

        Debug.Log("Subtract");
        IdleNumber.PrintIdleNumber(m_number_1 - m_number_2);
        IdleNumber.PrintIdleNumber(m_number_2 - m_number_1);

        Debug.Log("Multiply");
        IdleNumber.PrintIdleNumber(m_number_1 * m_multiplyFactor);

        Debug.Log("Divide");
        IdleNumber.PrintIdleNumber(m_number_1 / m_divideFactor);


        Debug.Log("Unit scale test ==================================");
        Debug.Log(IdleNumber.DetermineUnitScale(0));    //nothing
        Debug.Log(IdleNumber.DetermineUnitScale(17));   //= ar
        Debug.Log(IdleNumber.DetermineUnitScale(25));   //= az
        Debug.Log(IdleNumber.DetermineUnitScale(26));   //= ba
        Debug.Log(IdleNumber.DetermineUnitScale(51));   //= bz
        Debug.Log(IdleNumber.DetermineUnitScale(52));   //= ca


        Debug.Log("IDLE numbers text formating =======================");
        Debug.Log(IdleNumber.FormatIdleNumberText(m_number_1));
        Debug.Log(IdleNumber.FormatIdleNumberText(m_number_2));

    }

}
