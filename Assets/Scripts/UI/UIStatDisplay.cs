using UnityEngine;
using TMPro;
using GameDevTV.Inventories;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
using TkrainDesigns.Tiles.Stats;
using UnityEngine.Rendering;
using UnityEngine.UI;

#pragma warning disable CS0649
namespace RPG.UI
{
    public class UIStatDisplay : MonoBehaviour, IStatHolder
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI valueText;
        [SerializeField] ScriptableStat stat;
        [SerializeField] bool separateStatStore = false;
        [SerializeField] Button addButton;



        GameObject player = null;
        PersonalStats baseStats = null;
        Equipment equipment;
        StatStore statStore;

        public ScriptableStat GetStat()
        {
            return stat;
        }
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                baseStats = player.GetComponent<PersonalStats>();
                equipment = player.GetComponent<Equipment>();
                statStore = player.GetComponent<StatStore>();
                if (!statStore) print("No StatStore???");
            }
            if (titleText)
            {
                titleText.text = stat.Description;
            }
        }

        private void OnEnable()
        {
            equipment.EquipmentUpdated += OnSomethingChanged;
            statStore.onStatStoreUpdated += OnSomethingChanged;
            OnSomethingChanged();
        }

        private void OnDisable()
        {
            equipment.EquipmentUpdated += OnSomethingChanged;
        }

        public void AddStat()
        {
            statStore.IncreaseStatModifier(stat);
        }

        public void RemoveStat()
        {
            statStore.DecreaseStatModifier(stat);
        }

        void OnSomethingChanged()
        {

            if (baseStats && valueText)
            {
                int rawStat = (int) baseStats.GetStatValue(stat);
                int mods = statStore.GetPositiveModifier(stat);
                int netStat = rawStat - mods;
                string text = rawStat.ToString();
                if (separateStatStore)
                {
                    text = $"{netStat} (+{mods}) = {rawStat}";
                    addButton.gameObject.SetActive(statStore.IncreasesToSpend() > 0);
                }
                valueText.text = text;
            }
        }
    }
}