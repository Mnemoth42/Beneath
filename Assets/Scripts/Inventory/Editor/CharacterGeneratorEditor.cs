using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CharacterGenerator))]
public class CharacterGeneratorEditor :Editor
{
    SerializedProperty gender;
    SerializedProperty race;
    SerializedProperty hair;
    SerializedProperty eyebrow;
    SerializedProperty head;
    SerializedProperty facialHair;
    SerializedProperty defaultTorso;
    SerializedProperty defaultUpperArm;
    SerializedProperty defaultLowerArm;
    SerializedProperty defaultHand;
    SerializedProperty defaultHips;
    SerializedProperty defaultLegs;

    void OnEnable()
    {
        gender = serializedObject.FindProperty("gender");
        race = serializedObject.FindProperty("race");
        hair = serializedObject.FindProperty("hair");
        eyebrow = serializedObject.FindProperty("eyebrow");
        head = serializedObject.FindProperty("head");
        facialHair = serializedObject.FindProperty("facialHair");
        defaultTorso = serializedObject.FindProperty("defaultTorso");
        defaultUpperArm = serializedObject.FindProperty("defaultUpperArm");
        defaultLowerArm = serializedObject.FindProperty("defaultLowerArm");
        defaultHand = serializedObject.FindProperty("defaultHand");
        defaultHips = serializedObject.FindProperty("defaultHips");
        defaultLegs = serializedObject.FindProperty("defaultLeg");
    }

    public override void OnInspectorGUI()
    {
        
        EditorGUI.BeginChangeCheck();
        CharacterGenerator generator = (CharacterGenerator) target;
        EditorGUILayout.PropertyField(gender);
        EditorGUILayout.IntSlider(hair, -1, 37, "Hair");
        EditorGUILayout.IntSlider(head, 0, 21, "Head");
        EditorGUILayout.IntSlider(eyebrow, 0, 6, "Eyebrow");
        if (generator.isMale) EditorGUILayout.IntSlider(facialHair, 0, 17, "Facial Hair");
        EditorGUILayout.IntSlider(defaultTorso, 0, 27);
        EditorGUILayout.IntSlider(defaultUpperArm, 0, 20);
        EditorGUILayout.IntSlider(defaultLowerArm, 0, 17);
        EditorGUILayout.IntSlider(defaultHand, 0, 16);
        EditorGUILayout.IntSlider(defaultHips, 0, 27);
        EditorGUILayout.IntSlider(defaultLegs, 0, 18);

        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            generator.LoadDefaultCharacter();
        }

        

    
        


        if (GUILayout.Button("Requery"))
        {
            generator.InitGameObjects();
        }
    }
}
