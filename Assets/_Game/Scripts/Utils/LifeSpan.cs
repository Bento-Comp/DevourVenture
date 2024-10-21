using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpan : MonoBehaviour
{
    [SerializeField]
    private GameObject m_rootObject = null;

    [SerializeField]
    private float m_lifeSpan = 5f;

    private float m_timer;


    private void Start()
    {
        m_timer = 0f;
    }


    private void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer > m_lifeSpan)
            Destroy(m_rootObject);
    }

}
