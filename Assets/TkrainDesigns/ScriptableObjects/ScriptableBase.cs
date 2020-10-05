using UnityEngine;

namespace TkrainDesigns.ScriptableObjects
{
    public class ScriptableBase : ScriptableObject
    {

        
        public event System.Action OnValueChanged;

        protected void AnnounceEvent()
        {
            OnValueChanged?.Invoke();
        }
    } 
}
