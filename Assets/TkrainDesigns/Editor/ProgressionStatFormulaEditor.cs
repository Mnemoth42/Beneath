
//using UnityEngine;
//using UnityEditor;
//using RPG.Stats.Modifiers;
//using TkrainDesigns.ScriptableEnums;

//[CustomPropertyDrawer(typeof(StatFormula))]
//public class ProgressionStatFormulaEditor : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {

//        ScriptableStat stat = (ScriptableStat)property.FindPropertyRelative("stat").objectReferenceValue;
//        float startingValue = property.FindPropertyRelative("startingValue").floatValue;
//        float percentageAdded = property.FindPropertyRelative("percentageAdded").floatValue;
//        float absoluteAdded = property.FindPropertyRelative("absoluteAdded").floatValue;

//        StatFormula psf = new StatFormula(stat, startingValue, percentageAdded, absoluteAdded);
//        string level1 = string.Format("1:{0:F0}",psf.Calculate(1));
//        string level5 = string.Format("5:{0:F0}", psf.Calculate(5));
//        string level10 =string.Format("10:{0:F0}", psf.Calculate(10));
//        string Level25 =string.Format("25:{0:F0}", psf.Calculate(25));

//        string Attempt = stat == null ? "Stat" : stat.Description;
        
//        label = EditorGUI.BeginProperty(position, new GUIContent(Attempt), property);
//        Rect ContentPosition = EditorGUI.PrefixLabel(position, label);
//        Rect DisplayPosition = ContentPosition;
//        DisplayPosition.y += 18;
//        ContentPosition.height = 16;
//        DisplayPosition.height = 16;
        
//        EditorGUI.indentLevel = 0;
//        ContentPosition.width *= .25f;
//        EditorGUIUtility.labelWidth = 14f;
        
        
//        EditorGUI.PropertyField(ContentPosition, property.FindPropertyRelative("stat"), GUIContent.none);
//        ContentPosition.x += ContentPosition.width;
//        //ContentPosition.width *= .5f;
//        EditorGUI.DelayedFloatField(ContentPosition, property.FindPropertyRelative("startingValue"),new GUIContent("S"));
//        ContentPosition.x += ContentPosition.width;
//        EditorGUI.DelayedFloatField(ContentPosition, property.FindPropertyRelative("percentageAdded"), new GUIContent("P"));
//        ContentPosition.x += ContentPosition.width;
//        EditorGUI.DelayedFloatField(ContentPosition, property.FindPropertyRelative("absoluteAdded"), new GUIContent("A"));
//        DisplayPosition.width *= .25f;
//        EditorGUI.LabelField(DisplayPosition, level1);
//        DisplayPosition.x += DisplayPosition.width;
//        EditorGUI.LabelField(DisplayPosition, level5);
//        DisplayPosition.x += DisplayPosition.width;
//        EditorGUI.LabelField(DisplayPosition, level10);
//        DisplayPosition.x += DisplayPosition.width;
//        EditorGUI.LabelField(DisplayPosition, Level25);
//        EditorGUI.EndProperty();

//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return 16f+18f;
//    }

//}
