using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.Tiles.Combat
{
    [System.Serializable]
    public class AttackVariant
    {
        public RuntimeAnimatorController controller;
        public int baseDamage = 5;

#if UNITY_EDITOR
        public bool DrawInspector(ScriptableObject owner)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            GUIContent label = new GUIContent(controller!=null?controller.name:"Select Override", "Animator Override that will be used for this attack variation");
            var tempController =
                (RuntimeAnimatorController)EditorGUILayout.ObjectField(label, controller, typeof(RuntimeAnimatorController), false);
            label.text = "Base Damage";
            label.tooltip = "Used to calculate damage.";
            int damage = EditorGUILayout.IntSlider(label, baseDamage, 1, 100);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(owner, "Change Attack Variant");
                controller = tempController;
                baseDamage = damage;
                EditorUtility.SetDirty(owner);
            }

            bool result = GUILayout.Button("-");
            EditorGUILayout.EndHorizontal();
            return result;
        }

#endif

    } 
}
