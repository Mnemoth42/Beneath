using System.Runtime.Remoting.Messaging;
using TkrainDesigns.ScriptableObjects;
using UnityEngine;

namespace TkrainDesigns
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioRandomizer : MonoBehaviour
    {
        new AudioSource audio;
        [Header("Required, must be set")]
        public ScriptableAudioArray audioArray;
        [Header("If set, will check against CharacterGenerator for gender and play this if female.")]
        public ScriptableAudioArray femaleAudioArray;
        public float maxPitchRange = 1.4f;
        public float minPitchRange = .6f;
        public float spatialBlend = .95f;
        public bool playOnAwake;

        void Awake()
        {
            audio = GetComponent<AudioSource>();
            audio.playOnAwake = false;
            audio.spatialBlend = spatialBlend;
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

            if (femaleAudioArray != null)
            {
                CharacterGenerator generator = GetComponentInParent<CharacterGenerator>();
                if (generator)
                {
                    audio.PlayOneShot(generator.isMale?audioArray.GetRandomClip():femaleAudioArray.GetRandomClip());
                }
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