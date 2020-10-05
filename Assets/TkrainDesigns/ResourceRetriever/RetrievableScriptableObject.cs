using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.ResourceRetriever
{
    public abstract class RetrievableScriptableObject : ScriptableObject, IHasItemID, ISerializationCallbackReceiver
    {
        [Header("Unique UUID for saving references to this Scriptable Object", order = 0)]
        [SerializeField] string itemId;
        
        public string GetItemID()
        {
            return itemId;
        }

        public void ReIssueItemID()
        {
            itemId = Guid.NewGuid().ToString();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // Generate and save a new UUID if this is blank.
            if (string.IsNullOrWhiteSpace(itemId))
            {
                itemId = Guid.NewGuid().ToString();
            }
            // Test for multiple objects with the same UUID
            //var items = Resources.LoadAll<RetrievableScriptableObject>("").Where(p => p.GetItemID() == itemId).ToList();
            //if (items.Count > 1)
            //{
            //    itemId = Guid.NewGuid().ToString();
            //}
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Require by the ISerializationCallbackReceiver but we don't need
            // to do anything with it.
        }
    }
}
