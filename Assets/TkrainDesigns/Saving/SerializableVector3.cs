using UnityEngine;

namespace TkrainDesigns.Saving
{
    /// <summary>Vector3 that can be serialized and saved in a save file. </summary>
    [System.Serializable]
    public class SerializableVector3
    {
        float x;
        float y;
        float z;

        public SerializableVector3()
        {

        }


        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
        /// <summary>
        /// Converts to vector.
        /// </summary>
        /// <returns></returns>
        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
        }
    } 

    [System.Serializable]
    public class SerializableQuaternion
    {
        float x;
        float y;
        float z;
        float w;

        public SerializableQuaternion()
        {

        }

        public SerializableQuaternion(Quaternion quaternion)
        {
            x = quaternion.x;
            y = quaternion.y;
            z = quaternion.z;
            w = quaternion.w;
        }

        public Quaternion ToQuaternion()
        {
            return new Quaternion(x, y, z, w);
        }
    }
}
