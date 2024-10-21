using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyGainFx : MonoBehaviour
{
    [SerializeField]
    private GameObject m_rootObject = null;

    [SerializeField]
    private TMP_Text m_gainText = null;

    [SerializeField]
    private MoneyGainFx_AnimatorEvents m_moneyGainFx_AnimatorEvents = null;


    private void OnEnable()
    {
        Money_FxController.OnFxSpawn += OnFxSpawn;
        m_moneyGainFx_AnimatorEvents.OnAnimationEnd += OnAnimationEnd;
    }

    private void OnDisable()
    {
        Money_FxController.OnFxSpawn -= OnFxSpawn;
        m_moneyGainFx_AnimatorEvents.OnAnimationEnd -= OnAnimationEnd;
    }

    private void OnAnimationEnd()
    {
        Destroy(m_rootObject);
    }

    private void OnFxSpawn(GameObject fxReference, IdleNumber gains_Idlenumber)
    {
        if (fxReference == m_rootObject)
        {
            m_gainText.text = IdleNumber.FormatIdleNumberText(gains_Idlenumber);
        }
    }

}
