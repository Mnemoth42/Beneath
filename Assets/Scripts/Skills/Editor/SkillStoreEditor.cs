//using GameDevTV.Inventories;
//using UnityEditor;
//using UnityEngine;

//namespace TkrainDesigns.Tiles.Skills
//{
//    [CustomEditor(typeof(SkillStore))]
//    public class SkillStoreEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            SkillStore store = (SkillStore) target;
//            LearnableSkill skillToRemove = null;
//            GUIStyle style = new GUIStyle();
//            EditorGUILayout.BeginHorizontal();
//            GUILayout.FlexibleSpace();
//            EditorGUILayout.EndHorizontal();
//            float fixedWidth = GUILayoutUtility.GetLastRect().width*.33f;


//            foreach (LearnableSkill skill in store.Skills)
//            {
                
//                EditorGUILayout.BeginHorizontal();
//                EditorGUI.BeginChangeCheck();
//                int level = EditorGUILayout.IntField(skill.GetLevel());
//                int slot = EditorGUILayout.IntField( skill.GetSlot());
//                var items = Statics.GetScriptableObjectsOfType<ActionItem>();
//                var names = Statics.GetNamesFromScriptableObjectList(items);
//                int current=0;
//                if(skill.GetItem()!=null)
//                {
//                    current = Statics.FindNameInScriptableObjectList(names, skill.GetItem());
//                }

//                current = EditorGUILayout.Popup(current, names.ToArray());
//                if (EditorGUI.EndChangeCheck())
//                {
//                    skill.SetItem(items[current]);   
//                    skill.SetLevel(level);
//                    skill.SetSlot(slot);
//                }
//                if (GUILayout.Button("-"))
//                {
//                    skillToRemove = skill;
//                }
//                EditorGUILayout.EndHorizontal();
//            }

//            if (skillToRemove!=null)
//            {
//                store.RemoveSkill(skillToRemove);
//            }
//            if(GUILayout.Button("Add Skill"))
//            {
//                store.AddSkill(new LearnableSkill());
//            }
//        }
//    }
//}