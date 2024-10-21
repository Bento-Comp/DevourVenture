using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Manager_StandUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_standPivot = null;

    [SerializeField]
    private StandUI_ArrowImage m_arrowImage = null;

    [SerializeField]
    private RectTransform m_canvasReference = null;

    [SerializeField]
    private float m_upgradeUIOffsetScreenPercent = 0.075f;

    [SerializeField]
    private RectTransform m_standUIRectTransform = null;

    [SerializeField]
    private RectTransform m_standWindowRectTransform = null;

    [SerializeField]
    private float m_screenBorderOffset = 25f;

    private Stand m_selectedStand;
    private Vector3 m_standPosition;
    private Vector2 m_targetAnchoredPosition;
    private float m_canvasWidth;

    private void OnEnable()
    {
        Stand.OnStandSelected += OnStandSelected;
        Stand.OnStandUnlocked += OnStandUnlocked;
        Stand.OnShowUI += OnShowUI;
        HideUIArea.onClickHideUIArea += UnselectStand;
    }

    private void OnDisable()
    {
        Stand.OnStandSelected -= OnStandSelected;
        Stand.OnStandUnlocked -= OnStandUnlocked;
        Stand.OnShowUI -= OnShowUI;
        HideUIArea.onClickHideUIArea -= UnselectStand;
    }

    private void Start()
    {
        m_canvasWidth = m_canvasReference.sizeDelta.x;

        m_arrowImage.HideArrow();
    }

    private void LateUpdate()
    {
        if (m_selectedStand != null)
        {
            PositionUI(m_standPosition);
        }
    }

    private void OnStandUnlocked(Stand stand)
    {
        m_arrowImage.HideArrow();
    }

    private void OnShowUI(Stand.State state)
    {
        m_arrowImage.ShowArrow();
    }

    private void UnselectStand()
    {
        m_selectedStand = null;
        m_arrowImage.HideArrow();
    }

    private void OnStandSelected(Stand stand, Vector3 standPosition)
    {
        m_selectedStand = stand;
        m_standPosition = standPosition;

        PositionUI(m_standPosition);
    }

    private void PositionUI(Vector3 standWorldPosition)
    {
        Vector3 screenPos = Manager_Camera.Instance.Camera.WorldToScreenPoint(standWorldPosition);

        RectTransform arrowRectTransform = m_standPivot.GetComponent<RectTransform>();

        Camera screenSpaceCamera = Manager_ScreenSpaceCanvas.Instance.GetScreenSpaceCamera();

        Vector2 outVector2 = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(arrowRectTransform.parent as RectTransform, screenPos, screenSpaceCamera, out outVector2);

        m_targetAnchoredPosition = outVector2 + Vector2.up * m_upgradeUIOffsetScreenPercent * Screen.height;
        arrowRectTransform.anchoredPosition = m_targetAnchoredPosition;

        Vector2 standUIRectTransformPosition = Vector2.zero;
        float xDifference = 0;

        if (m_targetAnchoredPosition.x + (m_standWindowRectTransform.sizeDelta.x / 2f) > m_canvasWidth / 2f)
        {
            xDifference = m_targetAnchoredPosition.x + (m_standWindowRectTransform.sizeDelta.x / 2f) - (m_canvasWidth / 2f);
            xDifference *= -1;
            xDifference -= m_screenBorderOffset;
            standUIRectTransformPosition.x = xDifference;
        }

        if (m_targetAnchoredPosition.x - (m_standWindowRectTransform.sizeDelta.x / 2f) < -m_canvasWidth / 2f)
        {
            xDifference = m_targetAnchoredPosition.x - (m_standWindowRectTransform.sizeDelta.x / 2f) + (m_canvasWidth / 2f);
            xDifference *= -1;
            xDifference += m_screenBorderOffset;
            standUIRectTransformPosition.x = xDifference;
        }

        m_standUIRectTransform.anchoredPosition = standUIRectTransformPosition;
    }
}
