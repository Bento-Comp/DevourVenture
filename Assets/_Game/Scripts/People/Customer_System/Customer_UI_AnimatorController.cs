using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer_UI_AnimatorController : MonoBehaviour
{
    [SerializeField]
    private Customer m_customer = null;

    [SerializeField]
    private Animator m_animator = null;


    private void OnEnable()
    {
        m_customer.OnUpdateOrderUI += OnUpdateOrderUI;
    }

    private void OnDisable()
    {
        m_customer.OnUpdateOrderUI -= OnUpdateOrderUI;
    }

    private void OnUpdateOrderUI(Order orderReference)
    {
        PlayBounce();
    }

    private void PlayBounce()
    {
        m_animator.SetTrigger("Bounce");
    }

}
