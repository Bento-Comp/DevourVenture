using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Manager_Grid))]
public class Grid_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Manager_Grid managerGrid = (Manager_Grid)target;

        if (GUILayout.Button("Generate Grid"))
        {
            managerGrid.GenerateGrid();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            managerGrid.ClearGrid();
        }

        if (GUILayout.Button("Test Cell"))
        {
            managerGrid.TestCell();
        }
    }
}
