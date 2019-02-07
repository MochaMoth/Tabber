using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FedoraEssentials
{
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

    [CustomPropertyDrawer(typeof(TabberUseParent))]
    public class TabberUseParentEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect usePanelParentBox = new Rect(position.x, position.y, 15f, position.height);
            EditorGUI.PropertyField(usePanelParentBox, property.FindPropertyRelative("useParent"), GUIContent.none);

            if (property.FindPropertyRelative("useParent").boolValue)
            {
                Rect parentTransformBox = new Rect(position.x + usePanelParentBox.width, position.y, position.width - usePanelParentBox.width, position.height);
                EditorGUI.PropertyField(parentTransformBox, property.FindPropertyRelative("parent"), GUIContent.none);
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}