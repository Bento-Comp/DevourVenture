using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OrthographicCamera : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_canvasRectTransformTarget = null;

    [SerializeField]
    private Camera m_cameraToControl = null;


    private void Start()
    {
        float orthographicSize = m_canvasRectTransformTarget.sizeDelta.x * Screen.height / Screen.width * 0.5f;

        m_cameraToControl.orthographicSize = orthographicSize;
    }
}
