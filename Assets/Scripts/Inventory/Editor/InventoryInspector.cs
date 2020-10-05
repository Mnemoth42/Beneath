using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryItem), true)]
public class InventoryInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GUIStyle style = EditorStyles.foldout;
        style.fontStyle = FontStyle.Bold;
        InventoryItem item = (InventoryItem) target;
        GUIStyle content = new GUIStyle();
        content.fixedWidth = Screen.width - 25;
        EditorGUILayout.BeginVertical(content);
        item.DrawCustomInspector(Screen.width, style);
        EditorGUILayout.EndVertical();
        
    }

    

}
