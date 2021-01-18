
using GameDevTV.Inventories;
using UnityEditor;
using UnityEditor.Callbacks;

using UnityEngine;

namespace RPG.Inventory.Editor
{
    public class StatsEquipmentEditor : EditorWindow
    {
        InventoryItem selected = null;
        Vector2 scrollPosition;
        
        GUIStyle background;
        

        GUIStyle style;
        GUIStyle headerStyle;

        [MenuItem("Window/Equipment Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(StatsEquipmentEditor), false, "InventoryItem");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var candidate = EditorUtility.InstanceIDToObject(instanceID) as InventoryItem;
            if (candidate != null)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        void OnEnable()
        {
            Selection.selectionChanged += SelectionChanged;
            background = new GUIStyle();
            background.normal.background = EditorGUIUtility.Load("Assets/UI Assets/fantasy_gui_png/button_09.png") as Texture2D;
            background.padding = new RectOffset(30, 30, 30, 30);
            background.border = new RectOffset(0, 0, 0, 0);
        }

        void SelectionChanged()
        {
            var candidate = EditorUtility.InstanceIDToObject(Selection.activeInstanceID) as InventoryItem;
            if (candidate != null)
            {
                selected = candidate;
                Repaint();
            }
        }

        void OnDisable()
        {
            // ReSharper disable once DelegateSubtraction
            Selection.selectionChanged -= SelectionChanged;
        }

        int toolbarSel = 0;
        void OnGUI()
        {
            style = new GUIStyle(GUI.skin.label)
                    {
                        richText = true,
                        wordWrap = true,
                        stretchHeight = true,
                        fontSize = 18,
                        font = EditorGUIUtility.Load("Assets/Fonts/dum1.ttf") as Font,
                        alignment = TextAnchor.MiddleCenter
                    };
            headerStyle = new GUIStyle(style) {fontSize = 40};
            if (!selected)
            {
                EditorGUILayout.HelpBox("No StatsEquipableItem selected.", MessageType.Error);
                return;
            }

            

            Rect rect = new Rect(0,0,position.width*.65f, position.height);
            
            GUILayout.BeginArea(rect);
            DrawBasicInspector(rect);
            GUILayout.EndArea();
            rect.x = position.width *.66f;
            rect.width = position.width * .33f;
            DrawTooltip(rect);
        }

        void DrawBasicInspector(Rect rect)
        {
            rect.width -= 10;
            
            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontStyle = FontStyle.Bold;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar);
            GUIStyle contentStyle = new GUIStyle();
            contentStyle.fixedWidth = rect.width;
            GUILayout.BeginVertical(contentStyle);
            selected.DrawCustomInspector(position.width*.66f, foldoutStyle);
            GUILayout.EndVertical();
            
            EditorGUILayout.EndScrollView();
        }

        void DrawTooltip(Rect rect)
        {
            float iconSize = Mathf.Min(rect.width*.33f, rect.height*.33f);

            GUILayout.BeginArea(rect, background);
            EditorGUILayout.BeginVertical();
            if (selected.GetIcon()!=null)
            {
                Rect canvas = GUILayoutUtility.GetRect(iconSize, iconSize);
                GUI.DrawTexture(canvas, selected.GetIcon().texture, ScaleMode.ScaleToFit); 
            }

            style.fixedWidth = rect.width-60;
            headerStyle.fixedWidth = rect.width-60;
            EditorGUILayout.LabelField(selected.GetDisplayName(), headerStyle);
            EditorGUILayout.LabelField(selected.GetDescription(), style);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();

        }



       
    }
}