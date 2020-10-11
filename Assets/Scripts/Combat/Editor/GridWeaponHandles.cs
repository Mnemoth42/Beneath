using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.Tiles.Combat.Editor
{
    [CustomEditor(typeof(GridWeapon))]
    public class GridWeaponHandles : UnityEditor.Editor
    {
        protected void OnSceneGUI()
        {
            GridWeapon gridWeapon = (GridWeapon) target;
            EditorGUI.BeginChangeCheck();

            Vector3 newTargetPosition = Handles.PositionHandle(gridWeapon.GetEffectPosition(), gridWeapon.GetEffectRotation());
            Quaternion newTargetRotation = Handles.RotationHandle(gridWeapon.GetEffectRotation(), gridWeapon.GetEffectPosition());
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Move AimPoint");
                gridWeapon.SetEffectPoint(newTargetPosition);
                gridWeapon.SetEffectRotation(newTargetRotation);
            }
        }
    }
}
