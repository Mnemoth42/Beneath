using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Inventory.Editor
{
    public class DropLibraryEditor : EditorWindow
    {
        DropLibrary selected;

        [MenuItem("Window/DropLibary Editor")]
        public static void ShowEditorWindow()
        {
             GetDropLibraryEditor();
        }

        static DropLibraryEditor GetDropLibraryEditor()
        {
            return GetWindow(typeof(DropLibraryEditor), false, "DropLibary") as DropLibraryEditor;
        }

        public static void ShowEditorWindow(DropLibrary selection)
        {
            GetDropLibraryEditor().selected = selection;
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is DropLibrary library)
            {
                ShowEditorWindow(library);
                return true;
            }

            return false;
        }

        void OnSelectionChange()
        {
            if (EditorUtility.InstanceIDToObject(Selection.activeInstanceID) is DropLibrary library)
            {
                selected = library;
                Repaint();
            }
        }

        void OnGUI()
        {
            if (!selected)
            {
                EditorGUILayout.HelpBox("No Dialogue Selected", MessageType.Error);
                return;
            }
            EditorGUILayout.HelpBox($"{selected.name}", MessageType.Info);
            selected.DrawCustomInspector();
        }
    }
}