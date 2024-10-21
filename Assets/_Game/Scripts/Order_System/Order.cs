using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    Lemonade,
    Donut,
    HotDog,
    Coffee,
    Bagel,
    Cake,
    Juice,
    Noodles,
    Orange,
    RiceBall,
    Salad,
    Sandwich,
    Soda,
    Soup
}


[System.Serializable]
public class OrderAvailability
{
    public FoodType m_foodType;
    public bool m_isAvailable;
}

[System.Serializable]
public class Order
{
    public Counter m_counter;
    public List<Employee> m_employeeOnOrderList = new List<Employee>();
    public FoodType m_foodType;
    public int m_remainingQuantityToServe;
    public int m_onGoingQuantityToServe;

    public bool CanBeTaken { get => m_employeeOnOrderList.Count < m_remainingQuantityToServe - m_onGoingQuantityToServe; }

    public Order(Counter counter, FoodType orderType, int quantity)
    {
        m_counter = counter;
        m_foodType = orderType;
        m_remainingQuantityToServe = quantity;
    }


    public void AddEmployeeToTheCount(Employee employee)
    {
        if (!m_employeeOnOrderList.Contains(employee))
        {
            m_employeeOnOrderList.Add(employee);
        }
    }

    public void RemoveEmployeeFromTheCount(Employee employee)
    {
        if (m_employeeOnOrderList.Contains(employee))
            m_employeeOnOrderList.Remove(employee);
    }
}
