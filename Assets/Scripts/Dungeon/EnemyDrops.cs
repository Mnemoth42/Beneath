using System.Collections.Generic;
using TkrainDesigns.ResourceRetriever;
using TkrainDesigns.Tiles.Control;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.Dungeons
{
    [System.Serializable]
    public class EnemyDropEntry
    {
        public GameObject character = null;
        public AnimationCurve probability = new AnimationCurve();

        public EnemyDropEntry()
        {
            probability.AddKey(1, 1);
            probability.AddKey(100, 20);
        }
    }

    [CreateAssetMenu(fileName="New EnemyDropLibrary", menuName = "RPG/Enemy Drop Library")]
    public class EnemyDrops : RetrievableScriptableObject
    {
        [SerializeField]List<EnemyDropEntry> drops = new List<EnemyDropEntry>();

        public override string GetDescription()
        {
            return name;
        }

        public override string GetDisplayName()
        {
            return name;
        }

        public List<int> GetPotentialDrops(int level)
        {
            List<int> result = new List<int>();
            for(int d=0;d<drops.Count;d++)
            {
                EnemyDropEntry drop = drops[d];
                int probability = (int)drop.probability.Evaluate(level);
                if (drop.character == null) continue;
                for (int i = 1; i < probability; i++)
                {
                    result.Add(d);
                }
            }
            return result;
        }

        public GameObject GetDrop(int level = 1)
        {
            var potentialDrops = GetPotentialDrops(level);
            if (potentialDrops.Count == 0) return null;
            return drops[potentialDrops[Random.Range(0, potentialDrops.Count)]].character;
        }

#if UNITY_EDITOR

        void SetCharacter(int index, GameObject character)
        {
            if (character == drops[index].character) return;
            SetUndo("Change Character");
            drops[index].character = character;
            Dirty();
        }

        void SetProbability(int index, AnimationCurve probability)
        {
            SetUndo("Change Probability");
            drops[index].probability = probability;
            Dirty();
        }

        public void DrawInspector()
        {
            int itemToDelete = -1;
            for (int i = 0; i < drops.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                AIController enemy=null;
                if (drops[i].character != null) enemy = drops[i].character.GetComponent<AIController>();
                enemy = DrawObjectList("Enemy", enemy);
                AnimationCurve probablity = EditorGUILayout.CurveField( drops[i].probability);
                if (GUILayout.Button("-")) itemToDelete = i;
                if (EditorGUI.EndChangeCheck())
                {
                    SetCharacter(i,enemy.gameObject);
                    SetProbability(i,probablity);
                }
                EditorGUILayout.EndHorizontal();
            }

            if (itemToDelete > -1)
            {
                SetUndo("Delete Item");
                drops.RemoveAt(itemToDelete);
                Dirty();
            }

            if (GUILayout.Button("Add Enemy"))
            {
                SetUndo("Add Item");
                drops.Add(new EnemyDropEntry());
                Dirty();
            }
        }

#endif

    }
}