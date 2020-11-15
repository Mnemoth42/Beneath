using GameDevTV.Inventories;
using RPG.Stats.Modifiers;
using System.Collections.Generic;
using System.Linq;
using PsychoticLab;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Tiles.Stats;
using UnityEditor;
using UnityEngine;
#pragma warning disable CS0649
namespace RPG.Inventory
{


    [CreateAssetMenu(menuName = "RPG/Inventory/Equipable Item")]
	public class StatsEquipableItem : EquipableItem, IModifierProvider
	{
		

		[SerializeField] List<Modifier> additiveModifiers = new List<Modifier>();
		[SerializeField] List<Modifier> percentageModifiers = new List<Modifier>();


		
		public int AdditiveModifierCount => additiveModifiers.Count;
        public int PercentageModifierCount => percentageModifiers.Count;

#if UNITY_EDITOR
		public void SetAdditiveModifier(int index, Modifier modifier)
        {
            if (additiveModifiers[index].CompareModifier(modifier)) return;
			Undo.RecordObject(this, "Change Additive Modifier");
            additiveModifiers[index] = modifier;
            Dirty();
        }

        public void SetPercentageModifier(int index, Modifier modifier)
        {
            if (percentageModifiers[index].CompareModifier(modifier)) return;
			Undo.RecordObject(this, "Change Percentage Modifier");
            percentageModifiers[index] = modifier;
            Dirty();
        }

        public void NewAdditiveModifier()
        {
			Undo.RecordObject(this, "Add Additive Modifier");
            additiveModifiers.Add(new Modifier(null, 1));
            Dirty();
        }

        public void NewPercentageModifier()
        {
			Undo.RecordObject(this, "Add Percentage Modifier");
			percentageModifiers.Add(new Modifier(null, 1));
            Dirty();
        }

        public void RemovePercentageModifier(int index)
        {
			Undo.RecordObject(this, "Remove Percentage Modifier");
			percentageModifiers.RemoveAt(index);
            Dirty();
        }

        public void RemoveAdditiveModifier(int index)
        {
			Undo.RecordObject(this, "Remove Additive Modifier");
			additiveModifiers.RemoveAt(index);
            Dirty();
        }

        public Modifier GetAdditiveModifierAt(int index)
        {
            return additiveModifiers[index];
        }

        public Modifier GetPercentageModifierAt(int index)
        {
            return percentageModifiers[index];
        }

        bool drawStatsEquippableItem = false;
        public override void DrawCustomInspector(float width, GUIStyle style)
        {
            base.DrawCustomInspector(width, style);
            drawStatsEquippableItem =
                EditorGUILayout.Foldout(drawStatsEquippableItem, "StatsEquippableItem Data", style);
            if (!drawStatsEquippableItem) return;
           BeginIndent();
            int statToRemove = -1;
            for (int i = 0; i < additiveModifiers.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                ScriptableStat stat =
                    DrawScriptableObjectList(additiveModifiers[i].stat == null ? "Select Stat" : additiveModifiers[i].stat.DisplayName,
                                             additiveModifiers[i].stat);
                float value = EditorGUILayout.IntSlider("Value", (int) additiveModifiers[i].value, -10, 100);
                if (GUILayout.Button("-"))
                {
                    statToRemove = i;
                }
                SetAdditiveModifier(i, new Modifier(stat, value));
                EditorGUILayout.EndHorizontal();
            }
            
            if (GUILayout.Button("+ Additive Stat"))
            {
                NewAdditiveModifier();
            }
            if (statToRemove > -1)
            {
                RemoveAdditiveModifier(statToRemove);
                statToRemove = -1;
            }
            for (int i = 0; i < percentageModifiers.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                //ScriptableStat stat = (ScriptableStat)EditorGUILayout.ObjectField(percentageModifiers[i].stat == null ? "Select Stat" : percentageModifiers[i].stat.DisplayName,
                //                                                                  percentageModifiers[i].stat,
                //                                                                  typeof(ScriptableStat),
                //                                                                  false);
                ScriptableStat stat =
                    DrawScriptableObjectList(percentageModifiers[i].stat == null ? "Select Stat" : percentageModifiers[i].stat.DisplayName,
                                             percentageModifiers[i].stat);
                float value = EditorGUILayout.IntSlider("Value", (int)percentageModifiers[i].value, -10, 100);
                if (GUILayout.Button("-"))
                {
                    statToRemove = i;
                }
                SetPercentageModifier(i, new Modifier(stat, value));
                EditorGUILayout.EndHorizontal();
            }
            
            if (GUILayout.Button("+ Percentage Stat"))
            {
                NewPercentageModifier();
            }
            if (statToRemove > -1)
            {
                RemovePercentageModifier(statToRemove);
            }

            EndIndent();
        }

#endif

        public override int Price()
        {
            int result = base.Price();
            foreach (var pair in CombineDecoratorAndBuildStat(Decorator.AdditiveModifiers, additiveModifiers.ToArray()))
            {
                result += (int) Mathf.Max(pair.Value * Level * 2, 0);
            }

            foreach (var pair in CombineDecoratorAndBuildStat(Decorator.PercentageModifiers,
                                                              percentageModifiers.ToArray()))
            {
                result += (int) Mathf.Max(pair.Value * Level * 4,0);
            }
            return result;
        }

        public virtual IEnumerable<float> GetAdditiveModifier(ScriptableStat stat)
		{

			foreach (Modifier modifier in additiveModifiers.Where(m => m.stat == stat).ToArray())
			{
				yield return modifier.value+Level*.34f;
			}
			yield return Decorator.GetAdditiveModifier(stat);
		}

		public virtual IEnumerable<float> GetPercentageModifier(ScriptableStat stat)
		{
			
			foreach (Modifier modifier in percentageModifiers.Where(m => m.stat == stat).ToArray())
			{
				yield return modifier.value+ Level * .34f;
			}
			yield return Decorator.GetPercentageModifier(stat);
			
		}


		protected Dictionary<ScriptableStat, float> AdditiveTotals => CombineDecoratorAndBuildStat(Decorator.AdditiveModifiers, additiveModifiers.ToArray());

        protected Dictionary<ScriptableStat, float> PercentageTotals => CombineDecoratorAndBuildStat(Decorator.PercentageModifiers, percentageModifiers.ToArray());

        protected  Dictionary<ScriptableStat, float> CombineDecoratorAndBuildStat(Dictionary<ScriptableStat, float> decoratorStatModifiers, Modifier[] equipmentStatModifiers)
		{
			Dictionary<ScriptableStat, float> result = new Dictionary<ScriptableStat, float>();
			foreach (var pair in decoratorStatModifiers)
            {
                if (pair.Key == null) continue;
				result[pair.Key] = pair.Value;
			}
			foreach (var modifier in equipmentStatModifiers)
            {
                if (modifier.stat == null) continue;
				if (result.ContainsKey(modifier.stat))
				{
					result[modifier.stat] += modifier.value+Mathf.Floor(.34f*Level);
				}
				else
				{
					result[modifier.stat] = modifier.value+Mathf.Floor(.34f*Level);
				}
			}
			return result;
		}

		

		

        public override string GetDescription()
        {
            string result = $"{base.GetDescription()}\n";
            foreach (var pair in AdditiveTotals)
            {
                if (pair.Key != null) result += StatString(pair.Key.DisplayName, pair.Value, "point");
            }

            foreach (var pair in PercentageTotals)
            {
                if (pair.Key != null) result += StatString(pair.Key.DisplayName, pair.Value, "percent");
            }
            return result;
        }

		

	}

	
}
