using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TkrainDesigns.ResourceRetriever
{
    public class Statics : MonoBehaviour
    {
#if UNITY_EDITOR

        /// <summary>
        /// Gets a list of assets from the asset database in Assets/Game with the given type T component.  Component must exist in the root
        /// of the prefab, this method will not find child items.  It only searchs the Assets/Game directory.  You can override this behavior
        /// by changing the path in the AssetDatabase.FindAssets function call on the 3rd line of the function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetAssetsOfType<T>() where T : MonoBehaviour
        {
            List<T> itemList = new List<T>();
            string[] lGuids = AssetDatabase.FindAssets("t:GameObject", new string[] { "Assets/Game" });
            foreach (string id in lGuids)
            {
                string lAssetPath = AssetDatabase.GUIDToAssetPath(id);
                GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath<GameObject>(lAssetPath);
                if (go && go.TryGetComponent(out T test))
                {
                    itemList.Add(test);
                }
            }
            return itemList;
        }

        /// <summary>
        /// Gets a list of ScriptableObjects from the asset database in Assets/Game with the given type T component.  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetScriptableObjectsOfType<T>() where T : RetrievableScriptableObject
        {
            return Resources.LoadAll<T>("").ToList();
        }

        /// <summary>
        /// This will take a list of items of type T and return a list of strings with their corresponding names.  Use the result
        /// of <see cref="GetAssetsOfType{T}"/> to find all objects of a given type in the Game directory for a EditorGUILayout.Popup.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<string> GetNamesFromList<T>(List<T> list) where T : MonoBehaviour
        {
            List<string> result = new List<string>();
            foreach(T go in list)
            {
                result.Add(go.name);
            }
            return result;
        }

        /// <summary>
        /// This will take a list of items of type T and return a list of strings with their corresponding names.  Use the result
        /// of <see cref="GetScriptableObjectsOfType{T}"/> to find all objects of a given type in the Game directory for a EditorGUILayout.Popup.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<string> GetNamesFromScriptableObjectList<T>(List<T> list) where T : RetrievableScriptableObject
        {
            List<string> result = new List<string>();
            foreach(T go in list)
            {
                result.Add(go.GetDisplayName());
            }
            return result;
        }

        /// <summary>
        /// Returns the position of item's name in a list of strings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int FindNameInList<T>(List<string> list, T item) where T : MonoBehaviour
        {
            if (item == null) return 0;
            if (list.Contains(item.name))
            {
                return list.IndexOf(item.name);
            }
            return 0;
        }

        /// <summary>
        /// Returns the position of a RetrievableScriptableObject in a list of strings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int FindNameInScriptableObjectList<T>(List<string> list, T item) where T :RetrievableScriptableObject
        {
            if (item == null) return 0;
            if (list.Contains(item.GetDisplayName()))
            {
                return list.IndexOf(item.GetDisplayName());
            }
            return 0;
        }

#endif
    }
}