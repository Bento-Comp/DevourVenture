using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Machine : MonoBehaviour
{
    public enum State
    {
        Active,
        NotActive,
        ToUnlock
    }


    public System.Action<Machine.State> OnUpdateState;
    public static System.Action<Machine> OnMachineSelected;

    [SerializeField]
    private GridObjectPosition m_workerSpot = null;

    [SerializeField]
    private Collider m_interactableCollider = null;

    [SerializeField]
    private Collider m_activationCollider = null;

    [SerializeField]
    private GameObject m_spawnParticleFxPrefab = null;

    [SerializeField]
    private FoodType m_machineFoodType;


    private Employee m_employeeReference;
    private Machine.State m_state;
    private GameObject m_spawnParticleFx;
    private bool m_isMachineOccupiedByEmployee;
    private bool m_isMachineBookedByEmployee;


    public GridObjectPosition WorkerSpot { get => m_workerSpot; }
    public FoodType MachineFoodType { get => m_machineFoodType; }
    public State State1 { get => m_state; }
    public bool IsMachineBookedByWorker { get => m_isMachineBookedByEmployee; }
    public bool IsMachineOccupiedByWorker { get => m_isMachineOccupiedByEmployee; }


    private void OnEnable()
    {
        Stand.OnToggleMachineState += OnToggleMachineState;
        Manager_RaycastFromScreen.OnHitInteractableItem += OnHitInteractableItem;
    }

    private void OnDisable()
    {
        Stand.OnToggleMachineState -= OnToggleMachineState;
        Manager_RaycastFromScreen.OnHitInteractableItem -= OnHitInteractableItem;
    }


    private void OnToggleMachineState(Machine machineReference, Machine.State state)
    {
        if (machineReference == this)
        {
            m_state = state;
            OnUpdateState?.Invoke(state);

            m_interactableCollider.enabled = m_state == State.Active ? true : false;

            m_activationCollider.enabled = m_state == State.ToUnlock ? true : false;
        }
    }


    public void SetMachineBookedByEmployee(Employee employeeReference, bool state)
    {
        m_employeeReference = employeeReference;
        m_isMachineBookedByEmployee = state;
    }


    public void SetMachineOccupiedByEmployee(Employee employeeReference, bool state)
    {
        if (m_employeeReference == employeeReference)
            m_isMachineOccupiedByEmployee = state;
    }


    private void OnHitInteractableItem(Collider collider)
    {
        if (collider == m_interactableCollider)
        {
            OnMachineSelected?.Invoke(this);
        }

        if (collider == m_activationCollider)
        {
            UnlockMachine(true);
        }
    }

    private void UnlockMachine(bool isSpawningFx)
    {
        m_activationCollider.enabled = false;
        m_interactableCollider.enabled = true;

        m_state = State.Active;
        OnUpdateState?.Invoke(m_state);

        if (isSpawningFx)
            m_spawnParticleFx = Instantiate(m_spawnParticleFxPrefab, m_interactableCollider.transform.position + Vector3.up, Quaternion.identity);
    }

}
