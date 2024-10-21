using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Machine : UniSingleton.Singleton<Manager_Machine>
{
    [SerializeField]
    private List<Machine> m_machineList = null;


    public bool HasMachineAvailable(FoodType foodType)
    {
        for (int i = 0; i < m_machineList.Count; i++)
        {
            if (m_machineList[i].MachineFoodType == foodType)
            {
                if (!m_machineList[i].IsMachineBookedByWorker && !m_machineList[i].IsMachineOccupiedByWorker && m_machineList[i].State1 == Machine.State.Active)
                {
                    return true;
                }
            }
        }

        return false;
    }


    public Machine GetAvailableMachine(FoodType foodType)
    {
        if (m_machineList.Count <= 0)
        {
            Debug.LogError("No machine listed");
            return null;
        }

        for (int i = 0; i < m_machineList.Count; i++)
        {
            if (!m_machineList[i].IsMachineBookedByWorker && !m_machineList[i].IsMachineOccupiedByWorker && m_machineList[i].State1 == Machine.State.Active)
            {
                if (m_machineList[i].MachineFoodType == foodType)
                {
                    return m_machineList[i];
                }
            }
        }

        return null;
    }


}
