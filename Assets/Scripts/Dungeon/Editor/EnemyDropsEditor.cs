using TkrainDesigns.Dungeons;
using UnityEditor;
using UnityEditor.Callbacks;

namespace TkrainDesigns.Dungeons.Editor
{
    public class EnemyDropsEditor : EditorWindow
    {
        public EnemyDrops selected;

        [MenuItem("Window/EnemyDrops Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(EnemyDropsEditor), false, "Class Editor");
        }

        public static void ShowEditorWindow(EnemyDrops select)
        {
            var window = (EnemyDropsEditor)GetWindow(typeof(EnemyDropsEditor), false, "Class Editor");
            window.selected = select;
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var candidate = EditorUtility.InstanceIDToObject(instanceID) as EnemyDrops;
            if (candidate != null)
            {
                ShowEditorWindow(candidate);
                return true;
            }

            return false;
        }

        void OnSelectionChange()
        {
            var candidate = EditorUtility.InstanceIDToObject(Selection.activeInstanceID) as EnemyDrops;
            if (candidate != null)
            {
                selected= candidate;
                Repaint();
            }
        }

        void OnGUI()
        {
            if (selected == null)
            {
                EditorGUILayout.HelpBox("No EnemyDrops Selected", MessageType.Error);
                return;
            }
            EditorGUILayout.HelpBox(selected.name, MessageType.Info);
            selected.DrawInspector();

        }
    }
}

namespace TkrainDesigns.Tiles.Core.Editor
{
    [CustomEditor(typeof(EnemyDrops))]
    public class DropInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EnemyDrops selected = (EnemyDrops) target;
            selected.DrawInspector();
        }
    }

}
