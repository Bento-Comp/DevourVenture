using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_TransitionCounter : UniSingleton.Singleton<Manager_TransitionCounter>
{
    public static System.Action<Waiter, Counter> OnSendTransitionCounterWithOrder;
    public static System.Action<Employee, Counter> OnSendFreeTransitionCounterEmployeePosition;

    [SerializeField]
    private List<Counter> m_transitionCounterList = null;


    private void OnEnable()
    {
        //for employees producing food to drop the food on the counter
        Employee.OnAskFreeTransitionCounter += OnAskFreeTransitionCounter;
        Waiter.OnAskTransitionCounterWithFood += OnAskTransitionCounterWithFood;
    }

    private void OnDisable()
    {
        Employee.OnAskFreeTransitionCounter -= OnAskFreeTransitionCounter;
        Waiter.OnAskTransitionCounterWithFood -= OnAskTransitionCounterWithFood;
    }




    private void OnAskTransitionCounterWithFood(Waiter waiter)
    {
        Counter counter = GetAvailableTransitionCounterWaiterSpot();

        OnSendTransitionCounterWithOrder?.Invoke(waiter, counter);
    }

    private Counter GetAvailableTransitionCounterWaiterSpot()
    {
        for (int i = 0; i < m_transitionCounterList.Count; i++)
        {
            if (!m_transitionCounterList[i].IsCounterBookedByWaiter && !m_transitionCounterList[i].IsCounterOccupiedByWaiter)
                if (m_transitionCounterList[i].HasOrderToServe)
                    return m_transitionCounterList[i];
        }

        return null;
    }


    private void OnAskFreeTransitionCounter(Employee employee)
    {
        Counter counter = GetAvailableTransitionCounterEmployeeSpot();

        OnSendFreeTransitionCounterEmployeePosition?.Invoke(employee, counter);
    }

    private Counter GetAvailableTransitionCounterEmployeeSpot()
    {
        for (int i = 0; i < m_transitionCounterList.Count; i++)
        {
            if (!m_transitionCounterList[i].IsCounterBookedByEmployee && !m_transitionCounterList[i].IsCounterOccupiedByEmployee)
                if (!m_transitionCounterList[i].HasOrderToServe)
                    return m_transitionCounterList[i];
        }

        return null;
    }
}
