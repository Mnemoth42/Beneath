﻿using System.Collections.Generic;
using TkrainDesigns.Inventories;
using TkrainDesigns.ScriptableEnums;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.Tiles.Combat
{
    [CreateAssetMenu(fileName="New WeaponConfig", menuName="Inventory/WeaponConfig")]
    public class GridWeaponConfig : StatsEquipableItem
    {
        [SerializeField] GridWeapon model;
        [SerializeField] RuntimeAnimatorController controller;
        [SerializeField] float levelsBetweenIncrease = 1.0f;
        [SerializeField] float damage = 5.0f;
        [SerializeField] bool leftHanded = false;
        [SerializeField] ScriptableStat offensiveStat = null;
        [SerializeField] ScriptableStat defensiveStat = null;
        [SerializeField] List<float> stylesAndDamage=new List<float>();
        [SerializeField] List<AttackVariant> controllers = new List<AttackVariant>();


        public float Damage
        {
            get
            {
                if (controllers.Count > 0)
                {
                    return controllers[currentAttackForm].baseDamage+LevelBonus;
                }
                if (stylesAndDamage.Count == 0) return damage+LevelBonus;
                return stylesAndDamage[currentAttackForm]+LevelBonus;
            }
        }

        int currentAttackForm = 0;

        public int GetRandomAttackForm(Animator animator)
        {
            if (controllers.Count > 0)
            {
                currentAttackForm = Random.Range(0, controllers.Count);
                animator.runtimeAnimatorController = controllers[currentAttackForm].controller;
                return currentAttackForm;
            }
            if (stylesAndDamage.Count == 0) return 0;
            currentAttackForm = Random.Range(0, stylesAndDamage.Count);
            return currentAttackForm;
        }

        public GridWeapon EquipWeapon(Transform rightHand, Transform leftHand, GridWeapon oldWeapon, Animator animator)
        {
            if (oldWeapon!=null)
            {
                Destroy(oldWeapon.gameObject);
            }

            if (controllers.Count > 0)
            {
                animator.runtimeAnimatorController = controllers[0].controller;
            }
            else if (controller)
            {
                animator.runtimeAnimatorController = controller;
            }

            if (model)
            {
                GridWeapon weapon = Instantiate(model, leftHanded ? leftHand : rightHand);
                return weapon;
            }

            return null;
        }

        public ScriptableStat GetOffensiveStat()
        {
            return offensiveStat;
        }

        public ScriptableStat GetDefensiveStat()
        {
            return defensiveStat;
        }


        public override string GetDescription()
        {
            string result = base.GetDescription();
            result += "\n\n";
#if UNITY_EDITOR
            if (offensiveStat == null)
            {
                result += BadString("No Offenseive Stat Selected!")+"\n";
            }

            if (defensiveStat == null)
            {
                result += BadString("No Defensive Stat Selected!") + "\n";
            }
            
#endif
            if (controllers.Count==0)
            {
                if (stylesAndDamage.Count == 0)
                {
                    result += $"Base Damage: {damage+LevelBonus}";
                }
                else
                {
                    float min = 100;
                    float max = 0;
                    foreach (float style in stylesAndDamage)
                    {
                        min = Mathf.Min(style, min);
                        max = Mathf.Max(style, max);

                    }
                    result += $"Base Damage between {min+LevelBonus} and {max+LevelBonus}.";
                } 
            }
            else
            {
                float min = 100;
                float max = 0;
                foreach (AttackVariant varient in controllers)
                {
                    min = Mathf.Min(varient.baseDamage, min);
                    max = Mathf.Max(varient.baseDamage, max);
                }

                result += $"Base Damage between {min+LevelBonus} and {max+LevelBonus}";
            }

            return result;
        }

        int LevelBonus => Level / (int)levelsBetweenIncrease;

#if UNITY_EDITOR

        void SetDamage(float value)
        {
            if (damage == value) return;
            Undo.RecordObject(this, "Change Damage");
            damage = value;
            Dirty();
        }

        void SetGridWeapon(GridWeapon newWeapon)
        {
            if (model == newWeapon) return;
            Undo.RecordObject(this, "Change Weapon Model");
            model = newWeapon;
            Dirty();
        }

        void SetRuntimeAnimatorController(RuntimeAnimatorController newController)
        {
            if (controller == newController) return;
            Undo.RecordObject(this, "Change Controller");
            controller = newController;
            Dirty();
        }

        void SetLeftHanded(bool value)
        {
            if (leftHanded == value) return;
            Undo.RecordObject(this, value?"Set Left Handed":"Set Right Handed");
            leftHanded = value;
            Dirty();
        }

        void SetStyleDamage(int index, float newValue)
        {
            if (stylesAndDamage[index] == newValue) return;
            Undo.RecordObject(this, $"Change Style {index} Damage");
            stylesAndDamage[index] = newValue;
            Dirty();
        }

        void AddStyleDamage()
        {
            Undo.RecordObject(this, "Add Style");
            stylesAndDamage.Add(1.0f);
            Dirty();
        }

        void RemoveStyleDamage(int index)
        {
            Undo.RecordObject(this, "Remove Style");
            stylesAndDamage.RemoveAt(index);
            Dirty();
        }

        void SetOffensiveStat(ScriptableStat value)
        {
            if (offensiveStat == value) return;
            Undo.RecordObject(this, "Set Offensive Stat");
            offensiveStat = value;
            Dirty();
        }

        void SetDefensiveStat(ScriptableStat value)
        {
            if (defensiveStat == value) return;
            Undo.RecordObject(this, "Set Defensive Stat");
            defensiveStat = value;
            Dirty();
        }

        bool displayGridWeapon = true;
        public override void DrawCustomInspector(float width, GUIStyle style)
        {
            base.DrawCustomInspector(width, style);
            displayGridWeapon = EditorGUILayout.Foldout(displayGridWeapon, "GridWeapon Data", style);
            if (!displayGridWeapon) return;
            SetGridWeapon(DrawObjectListWithNull("Weapon Model", model));
            //SetGridWeapon((GridWeapon)EditorGUILayout.ObjectField("Weapon Model", model, typeof(GridWeapon), false));
            EditorGUILayout.BeginVertical(indent);
            DrawBoolSlider(ref leftHanded, "Left Handed", "Whether or not this equipment is attached to the left hand.");
            SetOffensiveStat(DrawScriptableObjectList("Offensive Stat", offensiveStat));
            SetDefensiveStat(DrawScriptableObjectList("Defensive Stat", defensiveStat));
            DrawFloatAsIntSlider(ref levelsBetweenIncrease, 1, 100, "Levels Between Increase");
            DrawStylesAndDamage();
            DrawAttackVariants();
            EditorGUILayout.EndVertical();
        }

        void DrawAttackVariants()
        {
            int controllerToDelete = -1;
            for (int i = 0; i < controllers.Count; i++)
            {
                if (controllers[i].DrawInspector(this))
                {
                    controllerToDelete = i;
                }
            }

            if (controllerToDelete > -1)
            {
                controllers.RemoveAt(controllerToDelete);
            }

            if (GUILayout.Button("Add Controller/Damage pair"))
            {
                controllers.Add(new AttackVariant());
            }
        }

        void DrawStylesAndDamage()
        {
            if (controllers.Count>0) return;
            SetRuntimeAnimatorController((RuntimeAnimatorController)EditorGUILayout.ObjectField("AnimatorOverride", controller, typeof(RuntimeAnimatorController), false));
            if (stylesAndDamage.Count == 0)
            {
                SetDamage(EditorGUILayout.IntSlider("Damage", (int) damage, 1, 50));
            }
            else
            {
                int styleToDelete = -1;
                for (int i = 0; i < stylesAndDamage.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    SetStyleDamage(i, EditorGUILayout.IntSlider($"Style {i}:", (int) stylesAndDamage[i], 1, 50));
                    if (GUILayout.Button("-"))
                    {
                        styleToDelete = i;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (styleToDelete > -1)
                {
                    RemoveStyleDamage(styleToDelete);
                }
            }

            if (GUILayout.Button("Add Style"))
            {
                AddStyleDamage();
            }
        }

#endif

    }
}