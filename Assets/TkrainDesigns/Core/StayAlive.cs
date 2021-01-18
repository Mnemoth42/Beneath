using UnityEngine;

namespace TkrainDesigns.Core
{
    public class StayAlive : MonoBehaviour
    {
    
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }



    }
}
