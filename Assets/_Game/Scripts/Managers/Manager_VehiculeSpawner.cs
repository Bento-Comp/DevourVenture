using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_VehiculeSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform m_spawnTransform = null;

    [SerializeField]
    private GameObject m_vehiclePrefab = null;

    [SerializeField]
    private float m_minTimeToSpawn = 1f;

    [SerializeField]
    private float m_maxTimeToSpawn = 1f;


    private float m_spawnTime;
    private float m_timer;


    private void Start()
    {
        m_timer = 0f;
        m_spawnTime = Random.Range(m_minTimeToSpawn, m_maxTimeToSpawn);
    }


    private void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_spawnTime)
        {
            m_timer = 0f;

            m_spawnTime = Random.Range(m_minTimeToSpawn, m_maxTimeToSpawn);

            Instantiate(m_vehiclePrefab, m_spawnTransform.position, m_spawnTransform.rotation, m_spawnTransform);
        }
    }

}
