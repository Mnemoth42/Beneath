using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ColorParameterController : MonoBehaviour
{
    [SerializeField] int parameter;

    CharacterGenerator character;
    TextMeshProUGUI title;
    TextMeshProUGUI result;
    Image image;
    int value;
    string parameterString = "";



    void Awake()
    {
        character = FindObjectOfType<CharacterGenerator>();
        parameterString = SyntyStatics.GearColors[parameter];
        var textMeshes = GetComponentsInChildren <TextMeshProUGUI>();
        title = textMeshes[0];
        result = textMeshes[1];
        var images = GetComponentsInChildren<Image>();
        image = images[1];
        title.text = parameterString;
    }

    void DrawDisplay()
    {
        result.text = $"{value}";
       image.color = SyntyStatics.GetColor(SyntyStatics.GearColors[parameter], value);
    }

    void Start()
    {
        value = character.CycleColor(parameterString, value, 0);
        DrawDisplay();
    }

    public void IncreaseValue()
    {
        value = character.CycleColor(parameterString, value, 1);
        DrawDisplay();
    }

    public void DecreaseValue()
    {
        value = character.CycleColor(parameterString, value, -1);
        DrawDisplay();
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ColorParameterController))]
    public class ColorParameterControllerEditor : Editor
    {
        SerializedProperty parameter;

        void OnEnable()
        {
            parameter = serializedObject.FindProperty("parameter");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.IntSlider(parameter, 0, SyntyStatics.GearColors.Length - 1);
            serializedObject.ApplyModifiedProperties();
            ColorParameterController controller = (ColorParameterController) target;
            TextMeshProUGUI[] texts = controller.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = SyntyStatics.GearColors[parameter.intValue];
            texts[1].text =  "0";
            controller.name = texts[0].text;
        }
    }

#endif

}
