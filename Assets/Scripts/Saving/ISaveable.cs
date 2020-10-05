using System.Collections.Generic;
using UnityEngine;

namespace TkrainDesigns.Saving
{
    public interface ISaveable
    {
        /// <summary>
        /// When called, gather any variables that need to be saved into a container return the container as an object. 
        /// </summary>
        /// <returns></returns>
        SaveBundle CaptureState();

        /// <summary>
        /// bundle will contain the container saved in CaptureState() as an object. Cast to your container and restore your variables.
        /// </summary>
        /// <param name="bundle"></param>
        void RestoreState(SaveBundle bundle);
    }

    /// <summary>
    /// Special class for saving information with the ISaveable interface.
    /// </summary>
    [System.Serializable]
    public class SaveBundle: object
    {
        Dictionary<string, object> map = new Dictionary<string, object>();
       
        public SaveBundle GetSaveBundle(string key)
        {
            return map.ContainsKey(key) ? (SaveBundle)map[key] : new SaveBundle();
        }

        public void PutSaveBundle(string key, SaveBundle bundle)
        {
            map[key] = bundle;
        }

        public bool GetBool(string key, bool fallback = false)
        {
            return map.ContainsKey(key) ? (bool)map[key] : fallback;
        }

        public void PutBool(string key, bool val)
        {
            map[key] = val;
        }

        public int GetInt(string key, int fallback = 0)
        {
            return map.ContainsKey(key) ? (int)map[key] : fallback;
        }

        public void PutInt(string key, int val)
        {
            map[key] = val;
        }

        public float GetFloat(string key, float fallback = 0.0f)
        {
            return map.ContainsKey(key) ? (float)map[key] : fallback;
        }

        public void PutFloat(string key, float val)
        {
            map[key] = val;
        }

        public string GetString(string key, string fallback = "")
        {
            return map.ContainsKey(key) ? (string)map[key] : fallback;
        }

        public void PutString(string key, string val)
        {
            map[key] = val;
        }

        public Vector3 GetVector3(string key)
        {
            return GetVector3(key, new Vector3(0, 0, 0));
        }

        public Vector3 GetVector3(string key, Vector3 fallback)
        {
            if (map.ContainsKey(key))
            {
                SerializableVector3 temp = (SerializableVector3)map[key];
                return temp.ToVector();
            }
            else
            {
                return fallback;
            }
        }

        public object GetObject(string key, object fallback = null)
        {
            return map.ContainsKey(key) ? map[key] : fallback;
        }

        public void PutObject(string key, object item)
        {
            map.Add(key, item);
        }

        public void PutVector3(string key, Vector3 val)
        {
            map[key] = new SerializableVector3(val);
        }

        public Quaternion GetQuaternion(string key)
        {
            return GetQuaternion(key, new Quaternion(0, 0, 0,0));
        }

        public Quaternion GetQuaternion(string key, Quaternion fallback)
        {
            if (map.ContainsKey(key))
            {
                SerializableQuaternion temp = (SerializableQuaternion)map[key];
                return temp.ToQuaternion();
            }
            else
            {
                return fallback;
            }
        }

        public void PutQuaternion(string key, Quaternion val)
        {
            map[key] = new SerializableQuaternion(val);
        }

        public bool ContainsKey(string key)
        {
            return map.ContainsKey(key);
        }

        public T GetValue<T>(string key, T fallback = default)
        {
            if (map.ContainsKey(key))
            {
                try
                {
                    return (T)map[key];
                }
                catch
                {
                    return fallback;
                }
            }
            return fallback;
        }
    }
}
