using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Button_PressAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator = null;


    public void PlayPress()
    {
        m_animator.SetTrigger("Press");
    }

    public void PlayRelease()
    {
        m_animator.SetTrigger("Release");
    }
}
