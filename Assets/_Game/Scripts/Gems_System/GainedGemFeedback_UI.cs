using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainedGemFeedback_UI : MonoBehaviour
{
    public static System.Action OnAskGemPosition;

    [SerializeField]
    private RectTransform m_rectTransform = null;

    [SerializeField]
    private Animator m_animator = null;

    [SerializeField]
    private float m_speed = 50f;


    private RectTransform m_targetRectTransform;
    private bool m_isMoving;

    private void OnEnable()
    {
        Gems_UI.OnSendGemPosition += OnSendGemPosition;
    }

    private void OnDisable()
    {
        Gems_UI.OnSendGemPosition -= OnSendGemPosition;
    }


    private void Start()
    {
        OnAskGemPosition?.Invoke();
    }

    private void Update()
    {
        if (m_targetRectTransform != null && m_isMoving)
        {
            m_rectTransform.position = Vector3.MoveTowards(m_rectTransform.position, m_targetRectTransform.position, m_speed * Time.deltaTime);

            if(Vector3.Distance(m_rectTransform.position, m_targetRectTransform.position) < 0.01f)
            {
                m_animator.SetTrigger("BigBump");
                m_isMoving = false;

                Invoke("DestroyGameobject", 1f);
            }
        }
    }

    private void DestroyGameobject()
    {
        Destroy(gameObject);
    }


    private void OnSendGemPosition(RectTransform targetRectTransform)
    {
        if (targetRectTransform == null)
            return;

        m_targetRectTransform = targetRectTransform;
        m_isMoving = true;
    }


}
