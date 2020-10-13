using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace TkrainDesigns.ScriptableEnums.Editor
{
    public class ScriptableStatEditor : EditorWindow
    {
        ScriptableStat selectedScriptableStat = null;
        Vector2 scrollDescription;

        [MenuItem("Window/ScriptableStat Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(ScriptableStatEditor), false, "Stat Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var candidate = EditorUtility.InstanceIDToObject(instanceID) as ScriptableStat;
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
            var candidate = EditorUtility.InstanceIDToObject(Selection.activeInstanceID) as ScriptableStat;
            if (candidate != null)
            {
                selectedScriptableStat = candidate;
                Repaint();
            }
        }

        void OnDisable()
        {
            Selection.selectionChanged -= SelectionChanged;
        }

        void OnGUI()
        {
            if (selectedScriptableStat == null)
            {
                EditorGUILayout.HelpBox("No ScriptableStat Selected!", MessageType.Error);
                return;
            }
            EditorGUILayout.HelpBox($"{selectedScriptableStat.Description} - UUID={selectedScriptableStat.GetItemID()}", MessageType.Info);
            
            
            string newDescription = EditorGUILayout.TextArea(selectedScriptableStat.Description);
            string extendedDescription = EditorGUILayout.TextArea(selectedScriptableStat.ExtendedDescripton);
            selectedScriptableStat.SetDescription(newDescription);
            selectedScriptableStat.SetExtendedDescription(extendedDescription);
            selectedScriptableStat.SetMinimum(EditorGUILayout.FloatField("Minimum Value", selectedScriptableStat.Minumum));
            selectedScriptableStat.SetMaximum(EditorGUILayout.FloatField("Maximum", selectedScriptableStat.Maximum));

            int statToRemove = -1;
            for (int i=0; i < selectedScriptableStat.GetSources().Count;i++)
            {
                StatSource statSource = selectedScriptableStat.GetSources()[i];
                EditorGUILayout.BeginHorizontal();
                string statLabel = statSource.stat != null ? statSource.stat.Description : "Select Stat";
                    ScriptableStat stat =
                        (ScriptableStat) EditorGUILayout.ObjectField(statLabel,statSource.stat, typeof(ScriptableStat), false);
                    float effectPerLevel = EditorGUILayout.FloatField("Effect Per Level", statSource.effectPerLevel);
                    selectedScriptableStat.ChangeStatSource(i, new StatSource(stat, effectPerLevel));
                    if (GUILayout.Button("-"))
                {
                    statToRemove = i;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (statToRemove > -1)
            {
                selectedScriptableStat.RemoveStatSource(statToRemove);
            }

            if (GUILayout.Button("Add Stat Source"))
            {
                selectedScriptableStat.AddStatSource();
            }
            
        }
    }
}