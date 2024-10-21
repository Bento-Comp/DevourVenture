using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    //Look for free counter to pick up customer's order
    public static System.Action<Employee> OnAskCounterWithCustomer;
    public static System.Action<Employee> OnAskOrderToTakeInCharge;
    //Look for free serving counter to deposit the food
    public static System.Action<Employee> OnAskFreeTransitionCounter;
    public static System.Action<Employee, Order> OnFoodPrepared;

    public static System.Action<Counter> OnTakingOrder;
    public static System.Action<Employee, Order, Counter> OnServingOrder;
    public System.Action OnCustomerServed;

    public static System.Action<Machine> OnStartUsingMachine;
    public static System.Action<Machine> OnStopUsingMachine;
    public System.Action<FoodType> OnFoodGenerated;
    public System.Action OnFoodPutOnTransitionCounter;

    public System.Action<float> OnStartActivityTimer;
    public System.Action OnStopActivityTimer;
    public System.Action OnEmployeeInAction;
    public System.Action OnEmployeeNotInAction;



    [SerializeField]
    protected GameObject m_rootObject = null;

    [SerializeField]
    protected GridNavigationAgent m_navigation = null;

    [SerializeField]
    protected Transform m_foodVisualSocket = null;

    [SerializeField]
    protected GameObject m_giftModel = null;

    [SerializeField]
    protected GameObject m_bodyModel = null;

    [SerializeField]
    protected Collider m_activationCollider = null;

    [SerializeField]
    protected GameObject m_spawnParticleFxPrefab = null;

    [SerializeField]
    protected float m_requestRefreshRate = 0.5f;


    protected float m_timer;

    public float Timer { get => m_timer; }


    protected virtual void OnEnable()
    {
        Manager_RaycastFromScreen.OnHitInteractableItem += OnHitInteractableItem;

        Manager_GlobalUpgrades.OnDataLoaded += OnDataLoaded;
    }

    protected virtual void OnDisable()
    {
        Manager_RaycastFromScreen.OnHitInteractableItem -= OnHitInteractableItem;

        Manager_GlobalUpgrades.OnDataLoaded -= OnDataLoaded;
    }


    protected virtual void Initialize()
    {
        m_activationCollider.enabled = true;

        m_giftModel.SetActive(true);
        m_bodyModel.SetActive(false);
    }


    private void OnHitInteractableItem(Collider colliderHitByRayCast)
    {
        if (colliderHitByRayCast == m_activationCollider)
        {
            UnlockEmployee(true);
        }
    }

    protected virtual void UnlockEmployee(bool isSpawningFx)
    {
        OnEmployeeNotInAction?.Invoke();

        m_activationCollider.enabled = false;

        m_bodyModel.SetActive(true);
        m_giftModel.SetActive(false);

        if (isSpawningFx)
            Instantiate(m_spawnParticleFxPrefab, m_navigation.Position, Quaternion.identity);
    }

    private void OnDataLoaded()
    {
        UnlockEmployee(false);
    }
}
