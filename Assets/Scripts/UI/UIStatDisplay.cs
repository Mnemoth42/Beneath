using UnityEngine;
using TMPro;
using GameDevTV.Inventories;
using TkrainDesigns.ScriptableEnums;
using TkrainDesigns.Stats;
#pragma warning disable CS0649
namespace RPG.UI
{
    public class UIStatDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI valueText;
        [SerializeField] ScriptableStat stat;


        GameObject player = null;
        PersonalStats baseStats = null;
        Equipment equipment;


        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                baseStats = player.GetComponent<PersonalStats>();
                equipment = player.GetComponent<Equipment>();
            }
            if (titleText)
            {
                titleText.text = stat.Description;
            }
        }

        private void OnEnable()
        {
            equipment.EquipmentUpdated += OnSomethingChanged;
            OnSomethingChanged();
        }

        private void OnDisable()
        {
            equipment.EquipmentUpdated += OnSomethingChanged;
        }
        void OnSomethingChanged()
        {

            if (baseStats && valueText)
            {
                valueText.text = Mathf.RoundToInt(baseStats.GetStatValue(stat)).ToString();
            }
        }
    }
}