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
                style.padding=new RectOffset(5,15,0,0);
                EditorGUILayout.BeginVertical(style);
                selectedScriptableClass.SetDisplayName(EditorGUILayout.TextField("Display Name", selectedScriptableClass.GetDisplayName()));
                selectedScriptableClass.SetDescription(EditorGUILayout.TextField("Description",selectedScriptableClass.Description));
                int statToRemove = -1;
                for (int i=0;i<selectedScriptableClass.Formula.Count;i++)
                
                {
                    var formula = selectedScriptableClass.Formula[i];
                    
                    if (formula.Stat!=null)
                    {
                        GUIStyle hStyle = new GUIStyle();
                        hStyle.fixedWidth = position.width - 60.0f;
                        EditorGUILayout.BeginHorizontal(hStyle);
                        GUIStyle labelStyle = GUI.skin.label;
                        labelStyle.fixedWidth = position.width / 6.0f;
                        EditorGUILayout.LabelField($"Level 1: {Mathf.Floor(formula.Calculate(1))}"+
                                                   $" | Level 5: {Mathf.Floor(formula.Calculate(5))}"+
                                                    $" | Level 10: {Mathf.Floor(formula.Calculate(10))}"+
                                                    $" | Level 20: {Mathf.Floor(formula.Calculate(20))}"+
                                                    $" | Level 50: {Mathf.Floor(formula.Calculate(50))}"+
                                                    $" | Level 100:{Mathf.Floor(formula.Calculate(100))}");
                        EditorGUILayout.EndHorizontal();
                    }
                    
                    EditorGUILayout.BeginHorizontal();
                    string statDesc = formula.Stat == null ? "Select Stat" : formula.Stat.DisplayName;
                    EditorGUI.BeginChangeCheck();
                    ScriptableStat newStat = (ScriptableStat)EditorGUILayout.ObjectField($"{statDesc}" ,formula.Stat, typeof(ScriptableStat), false);
                    if (newStat == null) {statToRemove = i;}
                    else
                    {
                        formula.showCurve = EditorGUILayout.Toggle(formula.showCurve);
                        int maxValue = (int)newStat.Maximum;
                        int minValue = (int)newStat.Minumum;
                        float newStartValue=formula.GetStartValue();
                        float newEndValue=formula.GetEndValue();
                        AnimationCurve newCurve=new AnimationCurve();
                        if (!formula.showCurve)
                        {
                            newStartValue = EditorGUILayout.IntSlider( (int)formula.GetStartValue(), minValue, maxValue);
                            newEndValue = EditorGUILayout.IntSlider( (int)formula.GetEndValue(), minValue, maxValue); 
                        }
                        else
                        {
                            newCurve = EditorGUILayout.CurveField(formula.ValueCurve);
                        }
                        if (EditorGUI.EndChangeCheck() && newStat != null)
                        {
                            Undo.RecordObject(selectedScriptableClass, "Undo Formula Change");
                            formula.Stat = newStat;
                            if (!formula.showCurve)
                            {
                                formula.SetStartValue(newStartValue, newEndValue);
                            }
                            else
                            {
                                formula.ValueCurve = newCurve;
                            }
                            EditorUtility.SetDirty(selectedScriptableClass);
                        }
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