using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Camera : UniSingleton.Singleton<Manager_Camera>
{
    [SerializeField]
    private Camera m_camera = null;

    public Camera Camera { get => m_camera; }
}
