using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniActivation
{
    [ExecuteInEditMode()]
    [DefaultExecutionOrder(-32000)]
    [AddComponentMenu("UniActivation/NamedActivationsManager")]
    public class NamedActivationsManager : NamedActivationsRegister
    {
        // Use this with caution:
        // useful when you need to switch
        // between multiple instances
        // (ex: for Game Mode or AB tests)
        public bool canSwitchInstanceAtRuntime;

        public static NamedActivationsManager Instance { get; private set; }

        void OnEnable()
        {
            if(Instance == null || canSwitchInstanceAtRuntime)
            {
                Instance = this;
            }
            else
            {
#if UNITY_EDITOR
                if(Application.isPlaying == false)
                    return;
#endif

                Debug.LogWarning("A singleton can only be instantiated once!");
                Destroy(gameObject);
                return;
            }
        }

        void OnDisable()
        {
            if(Instance == this)
            {
                Instance = null;
            }
        }

#if UNITY_EDITOR
        void LateUpdate()
        {
            if(Application.isPlaying == false)
            {
                Instance = this;
            }
        }
#endif
    }
}