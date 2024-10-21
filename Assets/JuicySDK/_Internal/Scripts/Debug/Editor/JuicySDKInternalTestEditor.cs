using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JuicyInternal;

[CustomEditor(typeof(JuicySDKInternalTest))]
[CanEditMultipleObjects]
public class JuicySDKInternalTestEditor: Editor
{
    new JuicySDKInternalTest target;

    private void Awake()
    {
        target = (JuicySDKInternalTest)base.target;
        target.hideFlags = HideFlags.HideInInspector;
        
    }
}
