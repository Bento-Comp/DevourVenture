using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DefaultExecutionOrder(1)]
public class Gems_UI : MonoBehaviour
{
    public static System.Action<RectTransform> OnSendGemPosition;

    [SerializeField]
    private RectTransform m_gemRectTransform = null;

    [SerializeField]
    private TMP_Text m_gemsText = null;


    private void OnEnable()
    {
        Manager_Gems.OnUpdateGemsCount += OnUpdateGemsCount;
        GainedGemFeedback_UI.OnAskGemPosition += OnAskGemPosition;
    }

    private void OnDisable()
    {
        Manager_Gems.OnUpdateGemsCount -= OnUpdateGemsCount;
        GainedGemFeedback_UI.OnAskGemPosition -= OnAskGemPosition;
    }


    private void OnAskGemPosition()
    {
        OnSendGemPosition?.Invoke(m_gemRectTransform);
    }

    private void OnUpdateGemsCount(int gemsCount)
    {
        m_gemsText.text = gemsCount.ToString();
    }
}
