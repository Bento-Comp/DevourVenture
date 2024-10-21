using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Manager_GlobalUpgrades))]
public class Manager_GlobalUpgrades_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Manager_GlobalUpgrades managerGlobalUpgrades = (Manager_GlobalUpgrades)target;

        if (GUILayout.Button("Sort List"))
        {
            managerGlobalUpgrades.SortListByCost();
        }

        if (GUILayout.Button("Load scriptableobjects Data"))
        {
            managerGlobalUpgrades.LoadScriptableObjectsData();
            EditorUtility.SetDirty(managerGlobalUpgrades);
        }

    }
}