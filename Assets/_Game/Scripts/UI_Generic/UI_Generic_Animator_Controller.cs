using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Generic_Animator_Controller : MonoBehaviour
{
    public System.Action OnAnimationEnd_Disapear;

    [SerializeField]
    private Animator m_animator = null;




    public void Play_Reset()
    {
        m_animator.SetTrigger("Reset");
    }


    public void Play_Appear()
    {
        m_animator.SetTrigger("Appear");
    }


    public void Play_Disapear()
    {
        m_animator.SetTrigger("Disappear");
    }


    public void Broadcast_AnimationEndEvent_Disapear()
    {
        OnAnimationEnd_Disapear?.Invoke();
    }

}
