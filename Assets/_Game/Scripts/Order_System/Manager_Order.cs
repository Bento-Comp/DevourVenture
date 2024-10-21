using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Manager_Order : UniSingleton.Singleton<Manager_Order>
{
    public static System.Action<Counter, Order> OnOrderTaken;
    public static System.Action<Order> OnOrderServed;
    public static System.Action<Order, Vector3> OnGainOrderMoney;
    public static System.Action<List<FoodType>> OnSendAvailableFoodTypeList;
    public static System.Action<Employee, Order> OnSendOrderToTakeInCharge;


    [SerializeField]
    private List<OrderAvailability> m_orderAvailabilityList = null;

    [SerializeField]
    private float m_popularDishRate = 0.8f;

    [SerializeField]
    private int m_maxQuantityPerOrder = 3;


    private List<Order> m_orderList = new List<Order>();
    private FoodType m_popularDishFoodType;
    private bool m_hasPopularDishSkill;


    protected override void OnSingletonEnable()
    {
        base.OnSingletonEnable();
        Employee.OnTakingOrder += OnTakingOrder;
        Employee.OnServingOrder += OnServingOrder;
        Employee.OnAskOrderToTakeInCharge += OnAskOrderToTakeInCharge;
        Employee.OnFoodPrepared += OnFoodPrepared;

        Stand.OnStandUnlocked += OnStandUnlocked;

        ActiveSkill_PopularDish_Logic.OnStartPopularDishSkillEffect += OnStartPopularDishSkillEffect;
        ActiveSkill_PopularDish_Logic.OnStopPopularDishSkill += OnStopPopularDishSkill;
    }


    private void OnDisable()
    {
        Employee.OnTakingOrder -= OnTakingOrder;
        Employee.OnServingOrder -= OnServingOrder;
        Employee.OnAskOrderToTakeInCharge -= OnAskOrderToTakeInCharge;
        Employee.OnFoodPrepared -= OnFoodPrepared;

        Stand.OnStandUnlocked -= OnStandUnlocked;

        ActiveSkill_PopularDish_Logic.OnStartPopularDishSkillEffect -= OnStartPopularDishSkillEffect;
        ActiveSkill_PopularDish_Logic.OnStopPopularDishSkill -= OnStopPopularDishSkill;
    }


    private void Start()
    {
        for (int i = 0; i < m_orderAvailabilityList.Count; i++)
        {
            m_orderAvailabilityList[i].m_isAvailable = false;
        }

        BroadcastAvailableOrders();
    }


    private void BroadcastAvailableOrders()
    {
        List<FoodType> orderAvailabilityList = new List<FoodType>();

        for (int i = 0; i < m_orderAvailabilityList.Count; i++)
        {
            if (m_orderAvailabilityList[i].m_isAvailable)
            {
                orderAvailabilityList.Add(m_orderAvailabilityList[i].m_foodType);
            }
        }

        OnSendAvailableFoodTypeList?.Invoke(orderAvailabilityList);
    }


    private void OnStartPopularDishSkillEffect(FoodType popularFoodType)
    {
        m_popularDishFoodType = popularFoodType;
        m_hasPopularDishSkill = true;
    }


    private void OnStopPopularDishSkill()
    {
        m_hasPopularDishSkill = false;
    }


    public bool HasOrderAvailable()
    {
        int orderAvailableCount = 0;

        for (int i = 0; i < m_orderAvailabilityList.Count; i++)
        {
            if (m_orderAvailabilityList[i].m_isAvailable)
                orderAvailableCount++;
        }

        return orderAvailableCount > 0;
    }


    private void OnStandUnlocked(Stand stand)
    {
        for (int i = 0; i < m_orderAvailabilityList.Count; i++)
        {
            if (stand.StandFoodType == m_orderAvailabilityList[i].m_foodType && !m_orderAvailabilityList[i].m_isAvailable)
            {
                m_orderAvailabilityList[i].m_isAvailable = true;
                BroadcastAvailableOrders();
                return;
            }
        }
    }


    private void OnTakingOrder(Counter counterReference)
    {
        Order order = new Order(counterReference, PickRandomAvailableFoodType(), Random.Range(1, m_maxQuantityPerOrder + 1));
        counterReference.SetOrderToProduceReference(order);
        m_orderList.Add(order);
        OnOrderTaken?.Invoke(counterReference, order);
    }


    private void OnAskOrderToTakeInCharge(Employee employee)
    {
        Order orderToTakeInCharge = GetAvailableOrder(employee);

        OnSendOrderToTakeInCharge?.Invoke(employee, orderToTakeInCharge);
    }


    private Order GetAvailableOrder(Employee employee)
    {
        for (int i = 0; i < m_orderList.Count; i++)
        {
            if (m_orderList[i].CanBeTaken && Manager_Machine.Instance.HasMachineAvailable(m_orderList[i].m_foodType))
            {
                m_orderList[i].AddEmployeeToTheCount(employee);
                return m_orderList[i];
            }
        }

        return null;
    }


    public void DropOrder(Employee employee, Order orderToDrop)
    {
        for (int i = 0; i < m_orderList.Count; i++)
        {
            if (m_orderList[i] == orderToDrop)
            {
                m_orderList[i].RemoveEmployeeFromTheCount(employee);
            }
        }
    }


    private FoodType PickRandomAvailableFoodType()
    {
        if (m_hasPopularDishSkill && Random.Range(0f, 1f) < (m_popularDishRate - (1f / m_orderAvailabilityList.Count)))
        {
            return m_popularDishFoodType;
        }
        else
        {
            List<OrderAvailability> orderAvailabilityList = new List<OrderAvailability>();

            for (int i = 0; i < m_orderAvailabilityList.Count; i++)
            {
                if (m_orderAvailabilityList[i].m_isAvailable)
                    orderAvailabilityList.Add(m_orderAvailabilityList[i]);
            }

            int randomIndex = Random.Range(0, orderAvailabilityList.Count);

            return orderAvailabilityList[randomIndex].m_foodType;
        }
    }

    private void OnFoodPrepared(Employee employee, Order order)
    {
        order.m_onGoingQuantityToServe++;
        order.RemoveEmployeeFromTheCount(employee);
    }

    private void OnServingOrder(Employee employee, Order orderServed, Counter counterReference)
    {
        OnGainOrderMoney?.Invoke(orderServed, counterReference.CounterPosition.GetWorldPosition());
        orderServed.m_remainingQuantityToServe--;
        orderServed.m_onGoingQuantityToServe--;

        OnOrderServed?.Invoke(orderServed);

        if (orderServed.m_remainingQuantityToServe == 0 && m_orderList.Contains(orderServed))
        {
            counterReference.SetOrderToProduceReference(null);
            m_orderList.Remove(orderServed);
        }
    }

}
