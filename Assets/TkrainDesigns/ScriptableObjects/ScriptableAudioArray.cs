using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#pragma warning disable CS0649
namespace TkrainDesigns.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AudioArray", menuName = "Shared/AudioArray")]
    public class ScriptableAudioArray : ScriptableObject
    {

        [SerializeField] List<AudioClip> clips=new List<AudioClip>();

        public int Count
        {
            get
            {
                return clips.Count;
            }
        }

        public AudioClip GetClip(int clip)
        {
            return clips[Mathf.Clamp(clip, 0, clips.Count - 1)];
        }

        public AudioClip GetRandomClip()
        {
            return clips[Random.Range(0, clips.Count)];
        }

        public List<AudioClip> GetClips()
        {
            return clips;
        }

#if UNITY_EDITOR

        public void RemoveClip(AudioClip clip)
        {
            if (!clips.Contains(clip)) return;
            Undo.RecordObject(this, "Remove clip "+clip.name);
            clips.Remove(clip);
            EditorUtility.SetDirty(this);
        }

        public void AddClip(AudioClip clip)
        {
            string undo = "Add Clip ";
            if (clip) undo += clip.name;
            Undo.RecordObject(this, undo);
            clips.Add(clip);
            EditorUtility.SetDirty(this);
        }

#endif

    } 
}
