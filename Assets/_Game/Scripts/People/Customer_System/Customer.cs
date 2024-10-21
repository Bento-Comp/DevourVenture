using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CustomerState
{
    LookingForCounter,
    GoingToCounter,
    WaitingForOrder,
    Leaving
}


public class Customer : MonoBehaviour
{
    public static System.Action<Customer> OnAskFreeCounterCustomerSpot;
    public static System.Action OnCustomerLeaves;
    public static System.Action<Customer> OnCustomerLeaving;
    public System.Action<Order> OnUpdateOrderUI;

    [SerializeField]
    private GameObject m_rootObject = null;

    [SerializeField]
    private GridNavigationAgent m_navigation = null;

    [SerializeField]
    private float m_requestRefreshRate = 0.5f;


    private Order m_customerOrder;
    private Counter m_counterReference;
    private CustomerState m_customerState;
    private float m_timer;


    private void OnEnable()
    {
        Manager_Customer.OnCustomerSpawn += OnCustomerSpawn;
        Manager_Customer.OnSpawnPositionSent += OnSpawnPositionSent;
        Manager_Customer.OnExitPositionSent += OnExitPositionSent;
        Manager_Order.OnOrderTaken += OnOrderTaken;
        Manager_Order.OnOrderServed += OnOrderServed;

        Manager_Counter.OnSendFreeCounterCustomerPosition += OnSendFreeCounterCustomerPosition;
    }


    private void OnDisable()
    {
        Manager_Customer.OnCustomerSpawn -= OnCustomerSpawn;
        Manager_Customer.OnSpawnPositionSent -= OnSpawnPositionSent;
        Manager_Customer.OnExitPositionSent -= OnExitPositionSent;
        Manager_Order.OnOrderTaken -= OnOrderTaken;
        Manager_Order.OnOrderServed -= OnOrderServed;

        Manager_Counter.OnSendFreeCounterCustomerPosition -= OnSendFreeCounterCustomerPosition;
    }


    private void Update()
    {
        switch (m_customerState)
        {
            case CustomerState.LookingForCounter:
                TryToFindAvailableCounter();
                break;
            default:
                break;
        }
    }


    private void TryToFindAvailableCounter()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_requestRefreshRate)
        {
            m_timer = 0;
            OnAskFreeCounterCustomerSpot?.Invoke(this);
        }
    }


    private void OnSendFreeCounterCustomerPosition(Customer customer, Counter freeCounter)
    {
        if (customer != this)
            return;

        if (freeCounter == null)
            return;

        m_counterReference = freeCounter;

        

        m_counterReference.SetCounterBookedByCustomer(this, true);
        m_navigation.SetDestination(m_counterReference.CustomerSpot.XPosition, m_counterReference.CustomerSpot.YPosition);
        m_customerState = CustomerState.GoingToCounter;
        m_navigation.OnDestinationReached += OnReachCounter;
    }



    private void OnReachCounter()
    {
        m_counterReference.SetCounterOccupiedByCustomerState(this, true);
        m_navigation.OnDestinationReached -= OnReachCounter;
        m_customerState = CustomerState.WaitingForOrder;
        m_navigation.ObjectToLookAt(m_counterReference.gameObject);
    }


    private void OnCustomerSpawn(GameObject customerReference)
    {
        if (customerReference == m_rootObject)
            Initialize();
    }


    private void OnOrderServed(Order orderReference)
    {
        if (m_customerOrder == orderReference)
        {
            OnUpdateOrderUI?.Invoke(orderReference);

            if (orderReference.m_remainingQuantityToServe == 0)
            {
                m_customerState = CustomerState.Leaving;
                m_counterReference.SetCounterBookedByCustomer(this, false);
                m_counterReference.SetCounterOccupiedByCustomerState(this, false);
                OnCustomerLeaving?.Invoke(this);
            }
        }
    }

    private void OnExitPositionSent(Customer customerReference, int xPosition, int yPosition)
    {
        if (customerReference == this)
        {
            m_navigation.SetDestination(xPosition, yPosition);
            m_navigation.OnDestinationReached += OnReachExit;
        }
    }

    private void OnReachExit()
    {
        m_navigation.OnDestinationReached -= OnReachExit;
        OnCustomerLeaves?.Invoke();
        Destroy(m_rootObject);
    }


    private void OnOrderTaken(Counter counterReference, Order customerOrder)
    {
        if (m_counterReference == counterReference)
        {
            m_customerOrder = customerOrder;
            OnUpdateOrderUI?.Invoke(m_customerOrder);
        }
    }


    private void OnSpawnPositionSent(GameObject customerReference, int xPosition, int yPosition)
    {
        if (customerReference == m_rootObject)
            m_navigation.SetPosition(xPosition, yPosition);
    }


    private void Initialize()
    {
        m_customerState = CustomerState.LookingForCounter;
    }
}
