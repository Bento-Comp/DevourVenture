using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WaiterState
{
    Free,
    GoingToServingCounterToPickUpOrder,
    TakingOrder,
    GoingToTransitionCounter,
    GoingToServeCustomer,
    ServingCustomer,
    ToUnlock
}


public class Waiter : Employee
{
    //Look for serving counter with food to pick up and deliver to the customer
    public static System.Action<Waiter> OnAskTransitionCounterWithFood;


    [SerializeField]
    private float m_takingOrderTime = 2f;

    [SerializeField]
    private float m_servingTime = 0.2f;


    private Counter m_counterReference;
    private Order m_orderToManage;
    private WaiterState m_waiterState;


    protected override void OnEnable()
    {
        base.OnEnable();
        Manager_Waiter.OnWaiterSpawn += OnWaiterSpawn;
        Manager_Waiter.OnSpawnPositionSent += OnSpawnPositionSent;
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired += OnGlobalUpgradeAquired;

        Manager_Counter.OnSendCounterWithCustomerPosition += OnSendCounterWithCustomerPosition;
        Manager_TransitionCounter.OnSendTransitionCounterWithOrder += OnSendCounterWithOrder;
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        Manager_Waiter.OnWaiterSpawn -= OnWaiterSpawn;
        Manager_Waiter.OnSpawnPositionSent -= OnSpawnPositionSent;
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired -= OnGlobalUpgradeAquired;

        Manager_Counter.OnSendCounterWithCustomerPosition -= OnSendCounterWithCustomerPosition;
        Manager_TransitionCounter.OnSendTransitionCounterWithOrder -= OnSendCounterWithOrder;
    }


    private void Update()
    {
        switch (m_waiterState)
        {
            case WaiterState.Free:
                TryToFindTask();
                break;
            case WaiterState.TakingOrder:
                TakingOrder();
                break;
            case WaiterState.ServingCustomer:
                ServingCustomer();
                break;
            default:
                break;
        }
    }

    // INITIALIZATION ==========================================================

    private void OnWaiterSpawn(GameObject waiterReference)
    {
        if (waiterReference == m_rootObject)
            Initialize();
    }

    private void OnSpawnPositionSent(GameObject workerReference, int xPosition, int yPosition)
    {
        if (workerReference == m_rootObject)
            m_navigation.SetPosition(xPosition, yPosition);
    }

    protected override void Initialize()
    {
        m_waiterState = WaiterState.ToUnlock;

        base.Initialize();

        m_navigation.SetSpeedMultiplier(Manager_Waiter.Instance.WaiterSpeedMultiplier);
    }

    protected override void UnlockEmployee(bool isSpawningFx)
    {
        m_waiterState = WaiterState.Free;

        base.UnlockEmployee(isSpawningFx);
    }


    // UPGRADE MANAGEMENT ======================================================

    private void OnGlobalUpgradeAquired(GlobalUpgrade globalUpgrade)
    {
        if (globalUpgrade.m_bonus == Bonus.CashiersWalkFaster)
        {
            m_navigation.SetSpeedMultiplier(Manager_Waiter.Instance.WaiterSpeedMultiplier);
        }
    }


    // WAITER STATE MANAGEMENT =================================================


    private void TryToFindTask()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_requestRefreshRate)
        {
            m_timer = 0;

            if (!Manager_Order.Instance.HasOrderAvailable())
                return;

            //LOOKING FOR COUNTERS TO TAKE ORDERS FROM CUSTOMER
            OnAskCounterWithCustomer?.Invoke(this);

            if (m_waiterState != WaiterState.Free)
                return;

            OnAskTransitionCounterWithFood?.Invoke(this);
        }
    }


    private void OnSendCounterWithCustomerPosition(Employee employee, Counter counter)
    {
        if (employee != this)
            return;

        if (counter == null)
            return;


        m_counterReference = counter;
        m_counterReference.SetCounterBookedByWaiter(this, true);
        

        m_navigation.SetDestination(m_counterReference.WaiterSpot.XPosition, m_counterReference.WaiterSpot.YPosition);
        m_waiterState = WaiterState.GoingToServingCounterToPickUpOrder;
        m_navigation.OnDestinationReached += OnReachCounterToPickUpOrder;

        OnEmployeeInAction?.Invoke();
    }

    /// <summary>
    /// When waiter reached the counter with a customer
    /// </summary>
    private void OnReachCounterToPickUpOrder()
    {
        m_navigation.OnDestinationReached -= OnReachCounterToPickUpOrder;
        m_counterReference.SetCounterOccupiedByWaiter(this, true);
        m_navigation.ObjectToLookAt(m_counterReference.gameObject);
        m_waiterState = WaiterState.TakingOrder;
        m_timer = 0f;
        OnStartActivityTimer?.Invoke(m_takingOrderTime);
    }

    /// <summary>
    /// Taking the order of the customer
    /// </summary>
    private void TakingOrder()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_takingOrderTime)
        {
            m_timer = 0;
            OnStopActivityTimer?.Invoke();
            OnTakingOrder?.Invoke(m_counterReference);

            m_counterReference.SetCounterBookedByWaiter(this, false);
            m_counterReference.SetCounterOccupiedByWaiter(this, false);

            m_waiterState = WaiterState.Free;

            OnEmployeeNotInAction?.Invoke();
        }
    }


    private void OnSendCounterWithOrder(Waiter waiter, Counter counter)
    {
        if (waiter != this)
            return;

        if (counter == null)
            return;

        m_counterReference = counter;
        m_counterReference.SetCounterBookedByWaiter(this, true);
        
        m_navigation.SetDestination(m_counterReference.WaiterSpot.XPosition, m_counterReference.WaiterSpot.YPosition);
        m_waiterState = WaiterState.GoingToTransitionCounter;
        m_navigation.OnDestinationReached += OnReachTransitionCounter;

        OnEmployeeInAction?.Invoke();
    }

    /// <summary>
    /// When waiter reached the counter with a customer
    /// </summary>
    private void OnReachTransitionCounter()
    {
        m_counterReference.SetCounterOccupiedByWaiter(this, true);

        m_navigation.OnDestinationReached -= OnReachTransitionCounter;
        m_navigation.ObjectToLookAt(m_counterReference.gameObject);

        m_waiterState = WaiterState.GoingToServeCustomer;

        //Find the customer to deliver the food
        m_orderToManage = m_counterReference.OrderToServeReference;
        m_counterReference.SetCounterOccupiedByWaiter(this, false);
        m_counterReference.SetCounterBookedByWaiter(this, false);
        m_counterReference.SetOrderToServe(null);

        m_navigation.SetDestination(m_orderToManage.m_counter.WaiterSpot.XPosition, m_orderToManage.m_counter.WaiterSpot.YPosition);
        m_navigation.OnDestinationReached += OnReachServingCounter;

        FoodVisualAssets foodVisualAssets = Manager_FoodVisualAssets.Instance.GetFoodVisualAsset(m_orderToManage.m_foodType);
        Instantiate(foodVisualAssets.foodModel, m_foodVisualSocket.position, Quaternion.identity, m_foodVisualSocket);
        OnFoodGenerated?.Invoke(m_orderToManage.m_foodType);
    }


    /// <summary>
    /// When worker reaches the counter to serve the customer
    /// </summary>
    private void OnReachServingCounter()
    {
        m_navigation.OnDestinationReached -= OnReachServingCounter;
        m_navigation.ObjectToLookAt(m_orderToManage.m_counter.gameObject);
        m_waiterState = WaiterState.ServingCustomer;
        m_timer = 0f;
    }


    /// <summary>
    /// When worker serves the customer
    /// </summary>
    private void ServingCustomer()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_servingTime)
        {
            m_timer = 0f;

            OnServingOrder?.Invoke(this, m_orderToManage, m_orderToManage.m_counter);
            m_waiterState = WaiterState.Free;
            OnEmployeeNotInAction?.Invoke();

            OnCustomerServed?.Invoke();

            DestroyTransformChildren.DestroyAllTransformChildren(m_foodVisualSocket);
        }
    }
}
