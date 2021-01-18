using GameDevTV.Inventories;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ParameterController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI result;
    CharacterGenerator character;

    [SerializeField] int parameter = 0;
    int value = 0;
    string parameterString = "";

    void Awake()
    {
        var texts = GetComponentsInChildren<TextMeshProUGUI>();
        text = texts[0];
        result = texts[1];
        character = FindObjectOfType<CharacterGenerator>();
        parameterString = EquipableItem.Categories[parameter];
        value = parameter < 21 ? -1 : 0;
        text.text = parameterString;
        DrawDisplay();
    }

    void DrawDisplay()
    {
        result.text = $"{value}";
    }

    void Start()
    {
        value = character.SetParameter(parameterString, value, 0);
        DrawDisplay();
    }

    public void IncreaseValue()
    {
        value = character.SetParameter(parameterString, value, 1);
        DrawDisplay();
    }

    public void DecreaseValue()
    {
        value = character.SetParameter(parameterString, value, -1);
        DrawDisplay();
    }

    #if UNITY_EDITOR

    [CustomEditor(typeof(ParameterController))]
    public class ParameterControllerEditor : Editor
    {
        SerializedProperty parameter;

        void OnEnable()
        {
            parameter = serializedObject.FindProperty("parameter");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.IntSlider(parameter, 0, EquipableItem.Categories.Count - 1);
            serializedObject.ApplyModifiedProperties();
            ParameterController controller = (ParameterController) target;
            TextMeshProUGUI[] texts = controller.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = EquipableItem.Categories[parameter.intValue];
            texts[1].text = parameter.intValue < 21 ? "-1" : "0";
            controller.name = texts[0].text;
        }
    }

    #endif

}
