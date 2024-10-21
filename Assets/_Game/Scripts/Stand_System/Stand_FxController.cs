using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand_FxController : MonoBehaviour
{
    [SerializeField]
    private Stand m_stand = null;

    [SerializeField]
    private ParticleSystem m_coinFx = null;



    private void OnEnable()
    {
        m_stand.OnUpgradePurchased += OnUpgradePurchased;
    }

    private void OnDisable()
    {
        m_stand.OnUpgradePurchased -= OnUpgradePurchased;
    }

    private void Start()
    {
        m_coinFx.Stop();
    }

    private void OnUpgradePurchased()
    {
        m_coinFx.Play();
    }

}
