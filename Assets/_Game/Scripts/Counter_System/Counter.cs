using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField]
    private GridObjectPosition m_counterPosition = null;

    [SerializeField]
    private GridObjectPosition m_customerSpot = null;

    [SerializeField]
    private GridObjectPosition m_waiterSpot = null;

    [SerializeField]
    private GridObjectPosition m_workerSpot = null;

    [SerializeField]
    private GridObjectPosition m_employeeSpot = null;

    [SerializeField]
    private Transform m_foodOnCounterPosition = null;

    [SerializeField]
    private FoodVisualAssets_ScriptableObject m_foodVisualAssetsData = null;

    [SerializeField]
    private bool m_isDebugTested = false;

    private Customer m_customerReference;
    private Waiter m_waiterReference;
    private Worker m_workerReference;
    private Employee m_employeeReference;
    private Order m_orderToProduceReference;
    private Order m_orderToServeReference;
    private bool m_isCounterOccupiedByCustomer;
    private bool m_isCounterBookedByCustomer;
    private bool m_isCounterOccupiedByWaiter;
    private bool m_isCounterBookedByWaiter;
    private bool m_isCounterOccupiedByWorker;
    private bool m_isCounterBookedByWorker;
    private bool m_isCounterOccupiedByEmployee;
    private bool m_isCounterBookedByEmployee;


    public GridObjectPosition WorkerSpot { get => m_workerSpot; }
    public GridObjectPosition WaiterSpot { get => m_waiterSpot; }
    public GridObjectPosition EmployeeSpot { get => m_employeeSpot; }
    public GridObjectPosition CustomerSpot { get => m_customerSpot; }
    public GridObjectPosition CounterPosition { get => m_counterPosition; }
    public Order OrderReference { get => m_orderToProduceReference; }
    public Order OrderToServeReference { get => m_orderToServeReference; }
    public bool IsCounterOccupiedByCustomer { get => m_isCounterOccupiedByCustomer; }
    public bool IsCounterBookedByCustomer { get => m_isCounterBookedByCustomer; }
    public bool IsCounterOccupiedByWaiter { get => m_isCounterOccupiedByWaiter; }
    public bool IsCounterBookedByWaiter { get => m_isCounterBookedByWaiter; }
    public bool IsCounterOccupiedByWorker { get => m_isCounterOccupiedByWorker; }
    public bool IsCounterBookedByWorker { get => m_isCounterBookedByWorker; }
    public bool IsCounterOccupiedByEmployee { get => m_isCounterOccupiedByEmployee; }
    public bool IsCounterBookedByEmployee { get => m_isCounterBookedByEmployee; }

    public bool IsOrderRevealed { get => !(m_orderToProduceReference == null); }
    public bool HasOrderToServe { get => m_orderToServeReference != null; }


    //CUSTOMER
    public void SetCounterBookedByCustomer(Customer customerReference, bool state)
    {
        m_customerReference = customerReference;
        m_isCounterBookedByCustomer = state;
    }

    public void SetCounterOccupiedByCustomerState(Customer customerReference, bool state)
    {
        if (m_customerReference == customerReference)
        {
            m_isCounterOccupiedByCustomer = state;
            m_customerReference = null;
        }
    }

    //WAITER
    public void SetCounterBookedByWaiter(Waiter waiterReference, bool state)
    {
        m_waiterReference = waiterReference;
        m_isCounterBookedByWaiter = state;
    }

    public void SetCounterOccupiedByWaiter(Waiter waiterReference, bool state)
    {
        if (m_waiterReference == waiterReference)
            m_isCounterOccupiedByWaiter = state;
    }

    //WORKER
    public void SetCounterBookedByWorker(Worker workerReference, bool state)
    {
        m_workerReference = workerReference;
        m_isCounterBookedByWorker = state;
    }

    public void SetCounterOccupiedByWorkerState(Worker workerReference, bool state)
    {
        if (m_workerReference == workerReference)
            m_isCounterOccupiedByWorker = state;
    }


    //EMPLOYEE
    public void SetCounterBookedByEmployee(Employee employeeReference, bool state)
    {
        m_employeeReference = employeeReference;
        m_isCounterBookedByEmployee = state;
    }

    public void SetCounterOccupiedByEmployeeState(Employee employeeReference, bool state)
    {
        if (m_employeeReference == employeeReference)
            m_isCounterOccupiedByEmployee = state;
    }


    public void SetOrderToProduceReference(Order order)
    {
        m_orderToProduceReference = order;
    }

    public void SetOrderToServe(Order order)
    {
        m_orderToServeReference = order;

        if (order == null)
            DestroyTransformChildren.DestroyAllTransformChildren(m_foodOnCounterPosition);
        else
        {
            for (int i = 0; i < m_foodVisualAssetsData.m_foodVisualAssetsList.Count; i++)
            {
                if (order.m_foodType == m_foodVisualAssetsData.m_foodVisualAssetsList[i].foodType)
                {
                    Instantiate(m_foodVisualAssetsData.m_foodVisualAssetsList[i].foodModel, m_foodOnCounterPosition.position, Quaternion.identity, m_foodOnCounterPosition);
                    return;
                }
            }
        }
    }

}
