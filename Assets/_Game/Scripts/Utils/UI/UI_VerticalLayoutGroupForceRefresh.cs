using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_VerticalLayoutGroupForceRefresh : MonoBehaviour
{
    private VerticalLayoutGroup m_verticalLayoutGroup;


    private void Start()
    {
        m_verticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();
    }

    private void OnEnable()
    {
        if (m_verticalLayoutGroup == null)
            return;

        StartCoroutine(RefreshVerticalLayoutGroup());
    }

    private IEnumerator RefreshVerticalLayoutGroup()
    {
        m_verticalLayoutGroup.childControlHeight = true;
        yield return null;
        m_verticalLayoutGroup.childControlHeight = false;
    }
}
