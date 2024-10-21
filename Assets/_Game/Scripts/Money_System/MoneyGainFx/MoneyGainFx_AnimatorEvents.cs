using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyGainFx_AnimatorEvents : MonoBehaviour
{
    public System.Action OnAnimationEnd;

    public void OnAnimationOver()
    {
        OnAnimationEnd?.Invoke();
    }
}
