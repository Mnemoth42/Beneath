using TkrainDesigns.ScriptableObjects;
using UnityEngine;

namespace TkrainDesigns
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioRandomizer : MonoBehaviour
    {
        new AudioSource audio;
        public ScriptableAudioArray audioArray;
        public float maxPitchRange = 1.4f;
        public float minPitchRange = .6f;
        public bool playOnAwake;

        void Awake()
        {
            audio = GetComponent<AudioSource>();
            audio.playOnAwake = false;
            audio.spatialBlend = .95f;
        }

        void Start()
        {
            if (playOnAwake)
            {
                Play();
            }
        }

        public void Play()
        {
            audio.pitch = Random.Range(minPitchRange, maxPitchRange);
            if (!audioArray)
            {
                audio.Play();
                return;
            }

            if (audioArray.Count == 0)
            {
                audio.Play();
                return;
            }

            audio.PlayOneShot(audioArray.GetRandomClip());
        }

        public void PlayIfPositive(float test)
        {
            if (test > 0) Play();
        }

        public void PlayIfNegative(float test)
        {
            if (test < 0) Play();
        }

    }
}