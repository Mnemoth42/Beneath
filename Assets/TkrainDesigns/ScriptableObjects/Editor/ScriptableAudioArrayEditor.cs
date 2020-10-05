using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace TkrainDesigns.ScriptableObjects.Editor
{
    public class ScriptableAudioArrayEditor : EditorWindow
    {
        ScriptableAudioArray selected;

        [MenuItem("Window/AudioArray Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(ScriptableAudioArrayEditor), false, "AudioArray Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var candidate = EditorUtility.InstanceIDToObject(instanceID) as ScriptableAudioArray;
            if (candidate != null)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        void OnSelectionChange()
        {
            var candidate = EditorUtility.InstanceIDToObject(Selection.activeInstanceID) as ScriptableAudioArray;
            if (candidate != null)
            {
                selected = candidate;
                Repaint();
            }
        }

        public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
        {
            System.Reflection.Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            System.Reflection.MethodInfo method = audioUtilClass.GetMethod(
                                                                           "PlayClip",
                                                                           System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
                                                                           null,
                                                                           new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                                                                           null
                                                                          );
            method.Invoke(
                          null,
                          new object[] { clip, startSample, loop }
                         );
        }


        void OnGUI()
        {
            if (selected == null)
            {
                EditorGUILayout.HelpBox("No ScriptableAudioArray Selected", MessageType.Error);
                return;
            }
            EditorGUILayout.HelpBox(selected.name, MessageType.Info);
            AudioClip clipToRemove = null;
            foreach (AudioClip clip in selected.GetClips())
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(clip.name);
                if (GUILayout.Button("Play"))
                {
                    PlayClip(clip);
                }

                if (GUILayout.Button("-"))
                {
                    clipToRemove = clip;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (clipToRemove != null)
            {
                selected.RemoveClip(clipToRemove);
            }
            if (selected.Count>0 && GUILayout.Button("Play Random"))
            {
                AudioClip clip = selected.GetRandomClip();
                PlayClip(clip);
            }
            EditorGUILayout.LabelField("Drag and drop any clip to add to array.");
            CheckDragAndDrop();
        }

        void CheckDragAndDrop()
        {
            Event evt = Event.current;
            switch (evt.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    break;
                case EventType.DragPerform:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    DragAndDrop.AcceptDrag();
                    foreach (var dragged_object in DragAndDrop.objectReferences)
                    {
                        AudioClip clip = (AudioClip)dragged_object;
                        if (clip)
                        {
                            selected.AddClip(clip);
                        }
                    }

                    break;
            }
        }

    }
}