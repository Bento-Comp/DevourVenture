using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WorkerState
{
    Free,
    GoingToCounter,
    TakingOrder,
    LookingForMachine,
    GoingToMachine,
    WorkingOnMachine,
    GoingToServingCounter,
    ServingCustomer,
    ToUnlock
}


public class Worker : Employee
{
    [SerializeField]
    private GridObjectPosition m_mainCharacterSpawnPosition = null;

    [SerializeField]
    private float m_takingOrderTime = 2f;

    [SerializeField]
    private float m_machineProcessingTime = 2f;

    [SerializeField]
    private float m_servingTime = 2f;

    [SerializeField]
    private int m_maxAttemptsBeforeSwitchingToFreeState = 2;

    [SerializeField]
    private bool m_isMainCharacter = false;

    private Counter m_counterReference;
    private Order m_orderToManage;
    private Machine m_machine;
    private WorkerState m_workerState;
    private int m_attemptsBuffer;


    protected override void OnEnable()
    {
        base.OnEnable();

        if (m_isMainCharacter == false)
        {
            Manager_Worker.OnWorkerSpawn += OnWorkerSpawn;
            Manager_Worker.OnSpawnPositionSent += OnSpawnPositionSent;
        }
        else
        {
            base.UnlockEmployee(false);
            m_navigation.SetPosition(m_mainCharacterSpawnPosition.XPosition, m_mainCharacterSpawnPosition.YPosition);
        }

        Manager_Worker.OnSpeedMultiplierUpdated += OnSpeedMultiplierUpdated;

        Manager_Counter.OnSendCounterWithCustomerPosition += OnSendCounterWithCustomerPosition;
        Manager_Order.OnSendOrderToTakeInCharge += OnSendOrderToTakeInCharge;
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        if (m_isMainCharacter == false)
        {
            Manager_Worker.OnWorkerSpawn -= OnWorkerSpawn;
            Manager_Worker.OnSpawnPositionSent -= OnSpawnPositionSent;
        }

        Manager_Worker.OnSpeedMultiplierUpdated -= OnSpeedMultiplierUpdated;

        Manager_Counter.OnSendCounterWithCustomerPosition -= OnSendCounterWithCustomerPosition;
        Manager_Order.OnSendOrderToTakeInCharge -= OnSendOrderToTakeInCharge;
    }


    private void Update()
    {
        switch (m_workerState)
        {
            case WorkerState.Free:
                TryToFindTask();
                break;
            case WorkerState.TakingOrder:
                TakingOrder();
                break;
            case WorkerState.LookingForMachine:
                TryToFindAvailableMachine();
                break;
            case WorkerState.WorkingOnMachine:
                UsingMachine();
                break;
            case WorkerState.ServingCustomer:
                ServingCustomer();
                break;
            default:
                break;
        }
    }


    // INITIALIZATION ==========================================================

    private void OnWorkerSpawn(GameObject workerReference)
    {
        if (workerReference == m_rootObject)
            Initialize();
    }

    private void OnSpawnPositionSent(GameObject workerReference, int xPosition, int yPosition)
    {
        if (workerReference == m_rootObject)
            m_navigation.SetPosition(xPosition, yPosition);
    }

    protected override void Initialize()
    {
        m_workerState = WorkerState.ToUnlock;

        base.Initialize();
    }

    private void OnSpeedMultiplierUpdated()
    {
        float speedMultiplier = Manager_Worker.Instance.WorkerSpeedMultiplier;

        if (m_isMainCharacter)
            speedMultiplier += MainCharacterEquipedEquipment.Instance.GetEquipmentBonusValue(EquipmentEffect.MovementSpeed);

        m_navigation.SetSpeedMultiplier(speedMultiplier);
    }

    protected override void UnlockEmployee(bool isSpawningFx)
    {
        m_workerState = WorkerState.Free;

        base.UnlockEmployee(isSpawningFx);
    }


    // WORKER STATE MANAGEMENT =================================================


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

            if (m_workerState != WorkerState.Free)
                return;


            //LOOKING FOR ORDER TO PRODUCE
            OnAskOrderToTakeInCharge?.Invoke(this);

            if (m_workerState != WorkerState.Free)
                return;
        }
    }

    private void OnSendOrderToTakeInCharge(Employee employee, Order orderToTakeInCharge)
    {
        if (employee != this)
            return;

        if (orderToTakeInCharge == null)
            return;

        m_orderToManage = orderToTakeInCharge;
        m_workerState = WorkerState.LookingForMachine;
        OnEmployeeInAction?.Invoke();
    }

    private void OnSendCounterWithCustomerPosition(Employee employee, Counter counter)
    {
        if (employee != this)
            return;

        if (counter == null)
            return;

        m_counterReference = counter;
        m_counterReference.SetCounterBookedByWorker(this, true);
        m_counterReference.SetCounterOccupiedByWorkerState(this, true);

        m_navigation.SetDestination(m_counterReference.WorkerSpot.XPosition, m_counterReference.WorkerSpot.YPosition);
        m_navigation.OnDestinationReached += OnReachCounter;

        m_workerState = WorkerState.GoingToCounter;

        OnEmployeeInAction?.Invoke();
    }



    /// <summary>
    /// When worker reached the counter with a customer
    /// </summary>
    private void OnReachCounter()
    {
        m_navigation.OnDestinationReached -= OnReachCounter;
        m_navigation.ObjectToLookAt(m_counterReference.gameObject);

        m_workerState = WorkerState.TakingOrder;
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

            m_counterReference.SetCounterBookedByWorker(this, false);
            m_counterReference.SetCounterOccupiedByWorkerState(this, false);

            m_workerState = WorkerState.Free;

            OnEmployeeNotInAction?.Invoke();
        }
    }


    /// <summary>
    /// Try to find available machine to produce what the customer wants
    /// </summary>
    private void TryToFindAvailableMachine()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_requestRefreshRate)
        {
            m_timer = 0;
            m_machine = Manager_Machine.Instance.GetAvailableMachine(m_orderToManage.m_foodType);

            if (m_machine != null)
            {
                m_machine.SetMachineBookedByEmployee(this, true);
                m_navigation.SetDestination(m_machine.WorkerSpot.XPosition, m_machine.WorkerSpot.YPosition);
                m_workerState = WorkerState.GoingToMachine;
                m_navigation.OnDestinationReached += OnReachMachine;
                m_attemptsBuffer = 0;
            }
            else
            {
                m_attemptsBuffer++;
                if (m_attemptsBuffer >= m_maxAttemptsBeforeSwitchingToFreeState)
                {
                    if (m_orderToManage != null)
                    {
                        Manager_Order.Instance.DropOrder(this, m_orderToManage);
                        m_workerState = WorkerState.Free;
                        OnEmployeeNotInAction?.Invoke();
                        m_attemptsBuffer = 0;
                    }
                }
            }
        }
    }


    /// <summary>
    /// When worker reaches the machine
    /// </summary>
    private void OnReachMachine()
    {
        m_navigation.OnDestinationReached -= OnReachMachine;
        m_machine.SetMachineOccupiedByEmployee(this, true);
        m_workerState = WorkerState.WorkingOnMachine;
        m_navigation.ObjectToLookAt(m_machine.gameObject);

        OnStartUsingMachine?.Invoke(m_machine);
        m_machineProcessingTime = Manager_FoodStats.Instance.GetFoodStats(m_machine.MachineFoodType).m_productionTime / Manager_ProductionTimerMultiplier.Instance.GetProductionTimeMultiplier(m_machine.MachineFoodType);

        m_timer = 0f;
        OnStartActivityTimer?.Invoke(m_machineProcessingTime);
    }


    /// <summary>
    /// Using machine to get what the customer wants
    /// </summary>
    private void UsingMachine()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_machineProcessingTime)
        {
            m_timer = 0;
            OnStopActivityTimer?.Invoke();
            m_workerState = WorkerState.GoingToServingCounter;

            m_navigation.SetDestination(m_orderToManage.m_counter.WorkerSpot.XPosition, m_orderToManage.m_counter.WorkerSpot.YPosition);
            m_navigation.OnDestinationReached += OnReachServingCounter;

            m_machine.SetMachineBookedByEmployee(this, false);
            m_machine.SetMachineOccupiedByEmployee(this, false);
            OnStopUsingMachine?.Invoke(m_machine);

            FoodVisualAssets foodVisualAssets = Manager_FoodVisualAssets.Instance.GetFoodVisualAsset(m_orderToManage.m_foodType);
            Instantiate(foodVisualAssets.foodModel, m_foodVisualSocket.position, Quaternion.identity, m_foodVisualSocket);
            OnFoodGenerated?.Invoke(m_orderToManage.m_foodType);
        }
    }


    /// <summary>
    /// When worker reaches the counter to serve the customer
    /// </summary>
    private void OnReachServingCounter()
    {
        m_navigation.OnDestinationReached -= OnReachServingCounter;
        m_navigation.ObjectToLookAt(m_orderToManage.m_counter.gameObject);
        m_workerState = WorkerState.ServingCustomer;
        m_timer = 0f;
        OnFoodPrepared(this, m_orderToManage);
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
            m_workerState = WorkerState.Free;
            OnEmployeeNotInAction?.Invoke();

            OnCustomerServed?.Invoke();

            DestroyTransformChildren.DestroyAllTransformChildren(m_foodVisualSocket);
        }
    }


}
