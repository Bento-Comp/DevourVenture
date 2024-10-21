using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Counter : UniSingleton.Singleton<Manager_Counter>
{
    public static System.Action<Customer, Counter> OnSendFreeCounterCustomerPosition;
    public static System.Action<Employee, Counter> OnSendCounterWithCustomerPosition;


    [SerializeField]
    private List<Counter> m_counterList = null;


    private void OnEnable()
    {
        Employee.OnAskCounterWithCustomer += OnAskCounterWithCustomer;
        Customer.OnAskFreeCounterCustomerSpot += OnAskFreeCounterCustomerSpot;
    }

    private void OnDisable()
    {
        Employee.OnAskCounterWithCustomer -= OnAskCounterWithCustomer;
        Customer.OnAskFreeCounterCustomerSpot -= OnAskFreeCounterCustomerSpot;
    }


    private void OnAskCounterWithCustomer(Employee employee)
    {
        SendCounterWithCustomer(employee);
    }

    private void SendCounterWithCustomer(Employee employee)
    {
        Counter counterWithCustomer = GetAvailableCounterWorkerSpot();

        OnSendCounterWithCustomerPosition?.Invoke(employee, counterWithCustomer);
    }


    private void OnAskFreeCounterCustomerSpot(Customer customer)
    {
        SendFreeCustomerPosition(customer);
    }

    private void SendFreeCustomerPosition(Customer customer)
    {
        Counter freeCustomerSpot = GetAvailableCounterCustomerSpot();

        OnSendFreeCounterCustomerPosition?.Invoke(customer, freeCustomerSpot);
    }


    private Counter GetAvailableCounterCustomerSpot()
    {
        for (int i = 0; i < m_counterList.Count; i++)
        {
            if (!m_counterList[i].IsCounterBookedByCustomer && !m_counterList[i].IsCounterOccupiedByCustomer)
                return m_counterList[i];
        }

        return null;
    }

    private Counter GetAvailableCounterWorkerSpot()
    {
        for (int i = 0; i < m_counterList.Count; i++)
        {
            if (!m_counterList[i].IsCounterBookedByWorker && !m_counterList[i].IsCounterOccupiedByWorker
                && !m_counterList[i].IsCounterBookedByWaiter && !m_counterList[i].IsCounterOccupiedByWaiter
                && m_counterList[i].IsCounterOccupiedByCustomer)
                if (!m_counterList[i].IsOrderRevealed)
                {
                    return m_counterList[i];
                }
        }

        return null;
    }

}
