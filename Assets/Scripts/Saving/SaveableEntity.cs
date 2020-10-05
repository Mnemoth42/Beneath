using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;

namespace TkrainDesigns.Saving
{
    [System.Serializable]
    class TransformBundle : object
    {
        float tx;
        float ty;
        float tz;
        float rw;
        float rx;
        float ry;
        float rz;
    
        public Vector3 position
        {
            get
            {
                return new Vector3(tx, ty, tz);
            }
        }
        public Quaternion rotation
        {
            get
            {
                return new Quaternion(rx, ry, rz, rw);
            }
        }


        public TransformBundle()
        {

        }

        public TransformBundle(Transform t)
        {
            tx = t.position.x;
            ty = t.position.y;
            tz = t.position.z;
            rw = t.rotation.w;
            rx = t.rotation.x;
            ry = t.rotation.y;
            rz = t.rotation.z;
        }
        
    }

    /// <summary></summary>
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        static Dictionary<string, SaveableEntity> globalLookup= new Dictionary<string, SaveableEntity>();
        [SerializeField] string UniqueIdentifier = "";
        public bool bDoNotAdjustLocation = false;

        public void CloneUniqueIdentifier(SaveableEntity other)
        {
            UniqueIdentifier = other.GetUniqueIdentifier();
        }

        public string GetUniqueIdentifier()
        {
            return UniqueIdentifier;
        }

        public object CaptureState()
        {
            //print("Capturing state for " + GetUniqueIdentifier());
            //Dictionary<string, object> States = new Dictionary<string, object>();
            SaveBundle States = new SaveBundle();
            foreach (ISaveable element in GetComponents<ISaveable>())
            {
                string key = element.GetType().ToString();
                //print("Capturing "+name+"'s information from "+key);

                //States[key] = element.CaptureState();
                States.PutSaveBundle(key, element.CaptureState());

            }
            SaveBundle transformBundle = CreateTransformBundle();
            States.PutSaveBundle("Transform", transformBundle);
            //EnemerateThe(States);
            return States;
        }

        private SaveBundle CreateTransformBundle()
        {
            SaveBundle transformBundle = new SaveBundle();
            transformBundle.PutVector3("position", transform.position);
            transformBundle.PutQuaternion("rotation", transform.rotation);
            return transformBundle;
        }

        public void RestoreState(object savedState)
        {
            SaveBundle States = (SaveBundle)savedState;
            //print("Restoring state for " + GetUniqueIdentifier());
            //EnemerateThe(States);
            if (States.ContainsKey("Transform"))
            {
                RestoreTransform(States.GetSaveBundle("Transform"));
            }
            foreach (ISaveable element in GetComponents<ISaveable>())
            {
                string key = element.GetType().ToString();
                if (States.ContainsKey(key))
                {
                    //print("Restoring information to " + key);
                    element.RestoreState(States.GetSaveBundle(key));
                }
                else
                {
                    print("Anomoly, " + name + " has a " + key + " but there was no package in the state bundle.");
                }
            }


        }

        private void RestoreTransform(SaveBundle saveBundle)
        {
            if (bDoNotAdjustLocation) return;
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (agent) agent.enabled = false;

            transform.position = saveBundle.GetVector3("position");
            transform.rotation = saveBundle.GetQuaternion("rotation");
            if (agent) agent.enabled = true;
        }

        private static void EnemerateThe(Dictionary<string, object> States)
        {
            foreach (var listing in States)
            {
                print($"Found {listing.Key}");
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("UniqueIdentifier");
            if (string.IsNullOrEmpty(property.stringValue) | !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
            globalLookup[UniqueIdentifier] = this;
        }

        private bool IsUnique(string candidate)
        {
            if (globalLookup.ContainsKey(UniqueIdentifier))
            {
                return (globalLookup[UniqueIdentifier] == this) | (globalLookup[UniqueIdentifier]==null);
            }
            return true;
        }
#endif
    } 
}
