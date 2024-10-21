using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investor_Actor : MonoBehaviour
{
    public static System.Action OnInvestorClicked;

    [SerializeField]
    private GameObject m_rootObject = null;

    [SerializeField]
    private Collider m_selfInteractableCollider = null;


    private void OnEnable()
    {
        Manager_RaycastFromScreen.OnHitInteractableItem += OnHitInteractableItem;
        Manager_InvestorBonus.OnRemoveInvestor += OnRemoveInvestor;
    }

    private void OnDisable()
    {
        Manager_RaycastFromScreen.OnHitInteractableItem -= OnHitInteractableItem;
        Manager_InvestorBonus.OnRemoveInvestor -= OnRemoveInvestor;
    }


    private void OnHitInteractableItem(Collider interactableCollider)
    {
        if (interactableCollider != m_selfInteractableCollider)
            return;

        OnInvestorClicked?.Invoke();
    }


    private void OnRemoveInvestor()
    {
        Destroy(m_rootObject);
    }
}
