using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways, RequireComponent(typeof(ContentSizeFitter))]
public class UI_ContentSizeFitterForceRefresh : MonoBehaviour
{
    private ContentSizeFitter m_contentSizeFitter;

    private void Start()
    {
        m_contentSizeFitter = gameObject.GetComponent<ContentSizeFitter>();

        if (m_contentSizeFitter == null)
        {
            Debug.LogError("Could not get Content Size Fitter component");
        }
    }

    private void OnEnable()
    {
        if (m_contentSizeFitter != null)
            StartCoroutine(RefreshContentSizeFitterCoroutine());
    }

    private IEnumerator RefreshContentSizeFitterCoroutine()
    {
        m_contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        yield return null;
        m_contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

}
