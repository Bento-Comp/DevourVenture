using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_GamekitAutomaticStart : MonoBehaviour
{
    private void Start()
    {
        GameFramework.Game.Instance.GameStart();
    }
}
