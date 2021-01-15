using TkrainDesigns.Core.Interfaces;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Combat;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.Tiles.Actions
{
    public class OffensiveSpellBase : PerformableActionItem
    {
        [SerializeField] protected float baseDamage = 5.0f;
        [SerializeField] protected ScriptableStat damageStat;
        [SerializeField] protected ScriptableStat defenseStat;
        [SerializeField] protected ScriptableStat intelligenceStat;
        [SerializeField] protected int range = 3;

        protected int AdjustedDamage
        {
            get
            {
                if (currentUser.TryGetComponent(out PersonalStats stats))
                {
                    return (int) (baseDamage + (stats.Level / 2.0f));
                }
                return (int)baseDamage;
            }
        }

        protected Vector3 GetStartPosition()
        {
            Vector3 startPosition = currentUser.transform.position + Vector3.up;
            GridFighter fighter = currentUser.GetComponent<GridFighter>();
            if (fighter.GetRightHandTransform() != null)
            {
                startPosition = fighter.GetRightHandTransform().position;
            }

            return startPosition;
        }

        public override bool Use(GameObject user)
        {
            if (user.TryGetComponent(out IController controller))
            {
                return controller.SetCurrentActionItem(this);

            }

            return false;
        }

        #if UNITY_EDITOR

        void SetBaseDamage(float value)
        {
            if (baseDamage == value) return;
            Undo.RecordObject(this, "Change Base Damage");
            baseDamage = value;
            Dirty();
        }

        void SetDamageStat(ScriptableStat stat)
        {
            if (damageStat == stat) return;
            Undo.RecordObject(this, "Set Damage Stat");
            damageStat = stat;
            Dirty();
        }

        void SetDefenseStat(ScriptableStat stat)
        {
            if (defenseStat == stat) return;
            Undo.RecordObject(this, "Set Defense Stat");
            defenseStat = stat;
            Dirty();
        }

        void SetIntelligenceStat(ScriptableStat stat)
        {
            Undo.RecordObject(this, "Set Intelligence Stat");
            intelligenceStat = stat;
            Dirty();
        }

        void SetRange(int newRange)
        {
            Undo.RecordObject(this, "Set Range");
            range = newRange;
            Dirty();
        }

        bool drawOffensiveSpellbase = true;
        public override void DrawCustomInspector(float width, GUIStyle style)
        {
            base.DrawCustomInspector(width, style);
            drawOffensiveSpellbase = EditorGUILayout.Foldout(drawOffensiveSpellbase, "Offensive Spell Data", style);
            if (!drawOffensiveSpellbase) return;
            SetBaseDamage((float)EditorGUILayout.IntSlider("Base Damage", (int)baseDamage, 1, 100));
            SetRange(EditorGUILayout.IntSlider("Range", range, 0, 8));
            SetDamageStat(DrawScriptableObjectList("Damage Stat", damageStat));
            SetDefenseStat(DrawScriptableObjectList("Defense Stat", defenseStat));
            SetIntelligenceStat(DrawScriptableObjectList("Intelligence Stat",intelligenceStat));
        }


#endif


    }
}