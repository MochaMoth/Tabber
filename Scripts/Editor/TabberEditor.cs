using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tabber))]
public class TabberEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Cycle Forward"))
        {
            Tabber uiCycle = (Tabber)target;

            uiCycle.DeactivateAllPanels();
            if (EditorApplication.isPlaying)
                uiCycle.CyclePanelForwards();
            else
                uiCycle.CycleForwardEditor();
        }

        if (GUILayout.Button("Cycle Backward"))
        {
            Tabber uiCycle = (Tabber)target;

            uiCycle.DeactivateAllPanels();
            if (EditorApplication.isPlaying)
                uiCycle.CyclePanelBackwards();
            else
                uiCycle.CycleBackwardsEditor();
        }

        if (GUILayout.Button("Cycle To"))
        {
            Tabber uiCycle = (Tabber)target;

            uiCycle.DeactivateAllPanels();
            uiCycle.ActivateActivePanel();
        }
    }
}
