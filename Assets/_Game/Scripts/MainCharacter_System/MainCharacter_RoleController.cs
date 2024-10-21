using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainCharacterRole
{
    Worker,
    Chef
}

public class MainCharacter_RoleController : MonoBehaviour
{
    [SerializeField]
    private MainCharacterRole m_role;

    [SerializeField]
    private Worker m_workerRole = null;

    [SerializeField]
    private Chef m_chefRole = null;

    [SerializeField]
    private ActivityTimerUI m_activityTimerUI = null;

    [SerializeField]
    private FoodValueUI m_foodValueUI = null;

    [SerializeField]
    private ActivityFx m_activityFx = null;

    private void Awake()
    {
        m_workerRole.gameObject.SetActive(m_role == MainCharacterRole.Worker);
        m_chefRole.gameObject.SetActive(m_role == MainCharacterRole.Chef);

        if (m_role == MainCharacterRole.Worker)
        {
            m_activityTimerUI.Employee = m_workerRole;
            m_foodValueUI.EmployeeReference = m_workerRole;
            m_activityFx.Employee = m_workerRole;
        }
        else
        {
            m_activityTimerUI.Employee = m_chefRole;
            m_foodValueUI.EmployeeReference = m_chefRole;
            m_activityFx.Employee = m_chefRole;
        }

        m_activityTimerUI.Initialize();
        m_foodValueUI.Initialize();
        m_activityFx.Initialize();
    }


}
