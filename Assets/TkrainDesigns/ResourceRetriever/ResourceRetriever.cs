using System.Collections.Generic;
using UnityEngine;

namespace TkrainDesigns.ResourceRetriever
{
    public class ResourceRetriever<T> where T : ScriptableObject, IHasItemID
    {
        static Dictionary<string, T> itemLookupCache;
     
        /// <summary>
        /// Retrieves a ScriptableObject of type T from a Resource folder.
        /// SO must also implement the IHasItemID interface, to allow the retrieval of the UUID.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static T GetFromID(string itemID)
        {

            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, T>();
                var itemList = Resources.LoadAll<T>("");
                foreach (var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.GetItemID()))
                    {
                        Debug.LogError(string.Format("Looks like there's a duplicate ItemID for objects: {0} and {1}", itemLookupCache[item.GetItemID()], item));
                        continue;
                    }

                    itemLookupCache[item.GetItemID()] = item;
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
            return itemLookupCache[itemID];
        }

        /// <summary>
        /// Retrieves a ScriptableObject of type T from a Resource folder.  SO must implement IHasItemID interface.
        /// Null safe version, always returns a list with 1 element.  Use foreach(var I in result) to use the value if it is not null.
        /// The foreach loop will ignore a null result and move on.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static List<T> GetFromIdNullProof(string itemId)
        { 
            T item = GetFromID(itemId);
            List<T> list = new List<T>();
            if (item)
            {
                list.Add(item);
            }
            return list;
        }
    }

    

}