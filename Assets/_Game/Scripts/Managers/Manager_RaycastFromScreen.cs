using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_RaycastFromScreen : MonoBehaviour
{
    public static System.Action<Collider> OnHitInteractableItem;
    public static System.Action OnHitNothing;

    [SerializeField]
    private Camera m_camera = null;

    [SerializeField]
    private LayerMask m_effectiveLayer = 0;

    [SerializeField]
    private bool m_isDebugEnabled = true;

    private bool m_isRaycastingEnabled = true;

    private void OnEnable()
    {
        Manager_SceneManagement.OnEnterLevelForTheFirstTime += OnEnterLevelForTheFirstTime;
        Manager_OpenBusiness.OnBusinessStarted += OnBusinessStarted;
        Controller.OnTap += OnTap;
    }

    private void OnDisable()
    {
        Manager_SceneManagement.OnEnterLevelForTheFirstTime -= OnEnterLevelForTheFirstTime;
        Manager_OpenBusiness.OnBusinessStarted -= OnBusinessStarted;
        Controller.OnTap -= OnTap;
    }

    private void OnEnterLevelForTheFirstTime()
    {
        m_isRaycastingEnabled = false;
    }

    private void OnBusinessStarted()
    {
        m_isRaycastingEnabled = true;
    }

    private void OnTap(Vector3 cursorPosition)
    {
        if (m_isRaycastingEnabled == false)
        {
            Debug.Log("Raycasting is not enabled");
            return;
        }

        if (m_isDebugEnabled)
            Debug.Log("Try raycast");

        if (Game_UI.IsAnyUIOpen)
        {
            if (m_isDebugEnabled)
            {
                Debug.Log("Can't raycast from camera");
                Game_UI.ShowOpenedUI();
            }

            return;
        }


        Ray ray = m_camera.ScreenPointToRay(cursorPosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_effectiveLayer))
        {
            OnHitInteractableItem?.Invoke(hit.collider);

            if (m_isDebugEnabled)
                Debug.Log(hit.collider.name, hit.collider.gameObject);
        }
        else
        {
            OnHitNothing?.Invoke();
            if (m_isDebugEnabled)
                Debug.Log("Hit nothing");
        }
    }

}
