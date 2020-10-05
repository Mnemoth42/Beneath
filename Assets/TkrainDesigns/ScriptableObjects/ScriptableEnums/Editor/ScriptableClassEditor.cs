using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;

namespace TkrainDesigns.ScriptableEnums.Editor
{
    public class ScriptableClassEditor : EditorWindow
    {
        ScriptableClass selectedScriptableClass = null;
        Vector2 scrollDescription;

        [MenuItem("Window/ScriptableClass Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(ScriptableClassEditor), false, "Class Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var candidate = EditorUtility.InstanceIDToObject(instanceID) as ScriptableClass;
            if (candidate != null)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        void OnEnable()
        {
            Selection.selectionChanged += SelectionChanged;
        }

        void SelectionChanged()
        {
            var candidate = EditorUtility.InstanceIDToObject(Selection.activeInstanceID) as ScriptableClass;
            if (candidate != null)
            {
                selectedScriptableClass = candidate;
                Repaint();
            }
        }

        void OnDisable()
        {
            Selection.selectionChanged -= SelectionChanged;
        }

        const string SelectStat = "Select Stat";
        void OnGUI()
        {
            if (selectedScriptableClass == null)
            {
                EditorGUILayout.HelpBox("No ScriptableClass Selected", MessageType.Error);
            }
            else
            {
                EditorGUILayout.HelpBox($"{selectedScriptableClass.name} - UUID = {selectedScriptableClass.GetItemID()}", MessageType.Info);
                if (GUILayout.Button($"{selectedScriptableClass.GetItemID()} -- Press to change."))
                {
                    selectedScriptableClass.ReIssueItemID();
                    EditorUtility.SetDirty(selectedScriptableClass);
                    Repaint();
                }
                scrollDescription = EditorGUILayout.BeginScrollView(scrollDescription);
                GUIStyle style = new GUIStyle();
                style.fixedWidth = position.width - 10;
                EditorGUILayout.BeginVertical(style);
                selectedScriptableClass.Description = EditorGUILayout.
                    TextField(selectedScriptableClass.Description);
                int statToRemove = -1;
                for (int i=0;i<selectedScriptableClass.Formula.Count;i++)
                
                {
                    var formula = selectedScriptableClass.Formula[i];
                    EditorGUILayout.Separator();
                    if (formula.Stat!=null)
                    {
                        EditorGUILayout.BeginHorizontal();
                        //EditorGUILayout.LabelField($"{formula.Stat.Description}: ");
                        EditorGUILayout.LabelField($"Level 1: {Mathf.Floor(formula.Calculate(1))}");
                        EditorGUILayout.LabelField($"Level 5: {Mathf.Floor(formula.Calculate(5))}");
                        EditorGUILayout.LabelField($"Level 10: {Mathf.Floor(formula.Calculate(10))}");
                        EditorGUILayout.LabelField($"Level 20: {Mathf.Floor(formula.Calculate(20))}");
                        EditorGUILayout.LabelField($"Level 50: {Mathf.Floor(formula.Calculate(50))}"); 
                        EditorGUILayout.EndHorizontal();
                    }
                    
                    EditorGUILayout.BeginHorizontal();
                    string statDesc = formula.Stat == null ? "Select Stat" : formula.Stat.Description;
                    EditorGUI.BeginChangeCheck();
                    ScriptableStat newStat = (ScriptableStat)EditorGUILayout.ObjectField($"{statDesc}" ,formula.Stat, typeof(ScriptableStat), false);
                    if (newStat == null) statToRemove = i;
                    float newStartingValue = EditorGUILayout.FloatField("Base:", formula.startingValue);
                    float newAbsoluteAdded = EditorGUILayout.FloatField("Absolute: ", formula.absoluteAdded);
                    float newPercentageAdded = EditorGUILayout.FloatField("Percentage: ", formula.percentageAdded);
                    if (EditorGUI.EndChangeCheck() && newStat!=null)
                    {
                        Undo.RecordObject(selectedScriptableClass, "Undo Formula Change");
                        formula.Stat = newStat;
                        formula.startingValue = newStartingValue;
                        formula.absoluteAdded = newAbsoluteAdded;
                        formula.percentageAdded = newPercentageAdded;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (statToRemove >= 0)
                {
                    Undo.RecordObject(selectedScriptableClass, "Remove Stat");
                    selectedScriptableClass.Formula.RemoveAt(statToRemove);
                }
                ScriptableStat potentialStat = (ScriptableStat)EditorGUILayout.ObjectField($"Add Stat:", null, typeof(ScriptableStat), false);
                if (potentialStat!=null)
                {
                    selectedScriptableClass.AddFormula(potentialStat);
                    EditorUtility.SetDirty(selectedScriptableClass);
                    Repaint();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
            }
        }
    }
}