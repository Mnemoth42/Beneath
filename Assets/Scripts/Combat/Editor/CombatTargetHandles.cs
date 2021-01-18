using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.Tiles.Combat.Editor
{
    [CustomEditor(typeof(CombatTarget))]
    public class CombatTargetHandles : UnityEditor.Editor
    {
        protected void OnSceneGUI()
        {
            CombatTarget combatTarget = (CombatTarget) target;
            EditorGUI.BeginChangeCheck();
            
            Vector3 newTargetPosition = Handles.PositionHandle(combatTarget.aimPoint, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Move AimPoint");
                combatTarget.aimPoint = newTargetPosition;
                EditorUtility.SetDirty(combatTarget);
            }
        }
    }
}