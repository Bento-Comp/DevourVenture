using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ChefState
{
    Free,
    LookingForMachine,
    GoingToMachine,
    WorkingOnMachine,
    LookingForTransitionCounter,
    GoingToTransitionCounter,
    ToUnlock
}



public class Chef : Employee
{
    public static System.Action<Order> OnPutFoodOnServingCounter;

    [SerializeField]
    private GridObjectPosition m_mainCharacterSpawnPosition = null;

    [SerializeField]
    private float m_machineProcessingTime = 2f;

    [SerializeField]
    private int m_maxAttemptsBeforeSwitchingToFreeState = 2;

    [SerializeField]
    private bool m_isMainCharacter = false;

    private Order m_orderToManage;
    private Machine m_machine;
    private Counter m_counter;
    private ChefState m_chefState;
    private int m_attemptsBuffer;


    protected override void OnEnable()
    {
        base.OnEnable();

        if (m_isMainCharacter == false)
        {
            Manager_Chef.OnChefSpawn += OnChefSpawn;
            Manager_Chef.OnSpawnPositionSent += OnSpawnPositionSent;
        }
        else
        {
            base.UnlockEmployee(false);
            m_navigation.SetPosition(m_mainCharacterSpawnPosition.XPosition, m_mainCharacterSpawnPosition.YPosition);
        }

        Manager_Chef.OnSpeedMultiplierUpdated += OnSpeedMultiplierUpdated;
        Manager_TransitionCounter.OnSendFreeTransitionCounterEmployeePosition += OnSendFreeCounterEmployeePosition;
        Manager_Order.OnSendOrderToTakeInCharge += OnSendOrderToTakeInCharge;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (m_isMainCharacter == false)
        {
            Manager_Chef.OnChefSpawn -= OnChefSpawn;
            Manager_Chef.OnSpawnPositionSent -= OnSpawnPositionSent;
        }

        Manager_Chef.OnSpeedMultiplierUpdated -= OnSpeedMultiplierUpdated;
        Manager_TransitionCounter.OnSendFreeTransitionCounterEmployeePosition -= OnSendFreeCounterEmployeePosition;
        Manager_Order.OnSendOrderToTakeInCharge -= OnSendOrderToTakeInCharge;
    }

    private void Update()
    {
        switch (m_chefState)
        {
            case ChefState.Free:
                TryToFindTask();
                break;
            case ChefState.LookingForMachine:
                TryToFindAvailableMachine();
                break;
            case ChefState.WorkingOnMachine:
                UsingMachine();
                break;
            case ChefState.LookingForTransitionCounter:
                TryToFindTransitionCounter();
                break;
            default:
                break;
        }
    }


    // INITIALIZATION ==========================================================

    private void OnChefSpawn(GameObject workerReference)
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
        m_chefState = ChefState.ToUnlock;

        base.Initialize();
    }

    private void OnSpeedMultiplierUpdated()
    {
        float speedMultiplier = Manager_Chef.Instance.ChefSpeedMultiplier;

        if (m_isMainCharacter)
            speedMultiplier += MainCharacterEquipedEquipment.Instance.GetEquipmentBonusValue(EquipmentEffect.MovementSpeed);

        m_navigation.SetSpeedMultiplier(speedMultiplier);
    }

    protected override void UnlockEmployee(bool isSpawningFx)
    {
        m_chefState = ChefState.Free;

        base.UnlockEmployee(isSpawningFx);
    }


    // CHEF STATE MANAGEMENT =================================================


    private void TryToFindTask()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_requestRefreshRate)
        {
            m_timer = 0;

            if (!Manager_Order.Instance.HasOrderAvailable())
                return;


            //LOOKING FOR ORDER TO PRODUCE
            OnAskOrderToTakeInCharge?.Invoke(this);

            if (m_chefState != ChefState.Free)
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
        m_chefState = ChefState.LookingForMachine;
        OnEmployeeInAction?.Invoke();
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
                m_chefState = ChefState.GoingToMachine;
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
                        m_chefState = ChefState.Free;
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
        m_chefState = ChefState.WorkingOnMachine;
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

            m_machine.SetMachineBookedByEmployee(this, false);
            m_machine.SetMachineOccupiedByEmployee(this, false);
            OnStopUsingMachine?.Invoke(m_machine);


            FoodVisualAssets foodVisualAssets = Manager_FoodVisualAssets.Instance.GetFoodVisualAsset(m_orderToManage.m_foodType);
            Instantiate(foodVisualAssets.foodModel, m_foodVisualSocket.position, Quaternion.identity, m_foodVisualSocket);
            OnFoodGenerated?.Invoke(m_orderToManage.m_foodType);


            m_chefState = ChefState.LookingForTransitionCounter;

        }
    }


    private void TryToFindTransitionCounter()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_requestRefreshRate)
        {
            m_timer = 0;

            OnAskFreeTransitionCounter?.Invoke(this);
        }
    }

    private void OnSendFreeCounterEmployeePosition(Employee employee, Counter counter)
    {
        if (employee != this)
            return;

        if (counter == null)
            return;

        m_chefState = ChefState.GoingToTransitionCounter;
        m_counter = counter;

        m_counter.SetCounterBookedByEmployee(this, true);

        m_navigation.SetDestination(m_counter.EmployeeSpot.XPosition, m_counter.EmployeeSpot.YPosition);
        m_navigation.OnDestinationReached += OnReachTransitionCounter;
    }


    /// <summary>
    /// When worker reaches the counter to serve the customer
    /// </summary>
    private void OnReachTransitionCounter()
    {
        m_counter.SetCounterBookedByEmployee(this, false);
        m_counter.SetCounterOccupiedByEmployeeState(this, true);
        m_counter.SetCounterOccupiedByEmployeeState(this, false);
        m_counter.SetOrderToServe(m_orderToManage);
        OnFoodPutOnTransitionCounter?.Invoke();

        OnFoodPrepared(this, m_orderToManage);

        m_navigation.OnDestinationReached -= OnReachTransitionCounter;
        m_navigation.ObjectToLookAt(m_orderToManage.m_counter.gameObject);

        m_chefState = ChefState.Free;
        m_timer = 0f;

        DestroyTransformChildren.DestroyAllTransformChildren(m_foodVisualSocket);
    }
}
