using TkrainDesigns.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace TkrainDesigns.Tiles.Stats.UI
{
    public class UIStatConfirm : MonoBehaviour
    {
        StatStore store;
        Button button;
        void Awake()
        {
            store = GameObject.FindGameObjectWithTag("Player").GetComponent<StatStore>();
            
            button = GetComponent<Button>();
        }

        void OnEnable()
        {
            store.onStatStoreUpdated += OnStatStoreChanged;
            OnStatStoreChanged();
        }

        void OnDisable()
        {
            store.onStatStoreUpdated -= OnStatStoreChanged;
        }

        void OnStatStoreChanged()
        {
            button.interactable = store.Dirty;
        }

        public void Cancel()
        {
            store.RevertChanges();
        }

        public void Commit()
        {
            store.CommitChanges();
        }

    }
}