using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Data.Util;
using GameDevTV.Inventories;
using PsychoticLab;
using RPG.Inventory;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    static Dictionary<string, List<string>> characterParts;

    public static Dictionary<string, List<string>> CharacterParts
    {
        get
        {
            InitCharacterParts();
            return characterParts;
        }
    }

    public static readonly string[] AllGenderBodyParts = new string[]
                                                         {
                                                             "HeadCoverings_Base_Hair",
                                                             "HeadCoverings_No_FacialHair",
                                                             "HeadCoverings_No_Hair",
                                                             "All_01_Hair",
                                                             "Helmet",
                                                             "All_04_Back_Attachment",
                                                             "All_05_Shoulder_Attachment_Right",
                                                             "All_06_Shoulder_Attachment_Left",
                                                             "All_07_Elbow_Attachment_Right",
                                                             "All_08_Elbow_Attachment_Left",
                                                             "All_09_Hips_Attachment",
                                                             "All_10_Knee_Attachement_Right",
                                                             "All_11_Knee_Attachement_Left",
                                                             "Elf_Ear"
                                                         };

    public static readonly string[] FemaleBodyCategories = new string[]
                                                           {
                                                               "Female_Head_All_Elements",
                                                               "Female_Head_No_Elements",
                                                               "Female_01_Eyebrows",
                                                               "Female_03_Torso",
                                                               "Female_04_Arm_Upper_Right",
                                                               "Female_05_Arm_Upper_Left",
                                                               "Female_06_Arm_Lower_Right",
                                                               "Female_07_Arm_Lower_Left",
                                                               "Female_08_Hand_Right",
                                                               "Female_09_Hand_Left",
                                                               "Female_10_Hips",
                                                               "Female_11_Leg_Right",
                                                               "Female_12_Leg_Left",
                                                           };

    public static readonly string[] MaleBodyCategories = new string[]
                                                         {
                                                             "Male_Head_All_Elements",
                                                             "Male_Head_No_Elements",
                                                             "Male_01_Eyebrows",
                                                             "Male_02_FacialHair",
                                                             "Male_03_Torso",
                                                             "Male_04_Arm_Upper_Right",
                                                             "Male_05_Arm_Upper_Left",
                                                             "Male_06_Arm_Lower_Right",
                                                             "Male_07_Arm_Lower_Left",
                                                             "Male_08_Hand_Right",
                                                             "Male_09_Hand_Left",
                                                             "Male_10_Hips",
                                                             "Male_11_Leg_Right",
                                                             "Male_12_Leg_Left",
                                                         };

    Dictionary<string, List<GameObject>> characterGameObjects;
    Dictionary<string, List<GameObject>> CharacterGameObjects
    {
        get
        {
            InitGameObjects();
            return characterGameObjects;
        }
    }

    [SerializeField] Gender gender = Gender.Male;
    [SerializeField] Race race = Race.Human;
    [Range(0, 37)]
    [SerializeField] int hair = 0;
    [Range(0, 21)]
    [SerializeField] int head = 0;
    [Range(0, 6)]
    [SerializeField] int eyebrow = 0;
    [Range(0, 17)]
    [SerializeField] int facialHair = 0;
    [Range(0, 27)]
    [SerializeField] int defaultTorso = 1;
    [Range(0, 20)]
    [SerializeField] int defaultUpperArm = 0;
    [Range(0, 17)]
    [SerializeField] int defaultLowerArm = 0;
    [Range(0, 16)]
    [SerializeField] int defaultHand = 0;
    [Range(0, 27)]
    [SerializeField] int defaultHips = 0;
    [Range(0, 18)]
    [SerializeField] int defaultLeg = 0;


    Equipment equipment;

    void Awake()
    {
        equipment = GetComponent<Equipment>();
        equipment.EquipmentUpdated += LoadArmor;
    }



    public void LoadDefaultCharacter()
    {
        ActivateHair(hair);
        ActivateHead(head);
        ActivateEyebrows(eyebrow);
        ActivateFacialHair(facialHair);
        ActivateTorso(defaultTorso);
        ActivateUpperArm(defaultUpperArm);
        ActivateLowerArm(defaultLowerArm);
        ActivateHand(defaultHand);
        ActivateHips(defaultHips);
        ActivateLeg(defaultLeg);
    }

    void LoadArmor()
    {
        LoadDefaultCharacter();
        foreach (var pair in equipment.EquippedItems)
        {
            foreach (string category in pair.Value.SlotsToDeactivate)
            {
                DeactivateCategory(category);
            }

            foreach (ItemPair itemPair in pair.Value.ObjectsToActivate)
            {
                ActivatePart(itemPair.category, itemPair.index);
            }
        }
    }


    void ActivateLeg(int selector)
    {
        ActivatePart(gender == Gender.Male ? "Male_12_Leg_Left" : "Female_12_Leg_Left", selector);
        ActivatePart(gender == Gender.Male ? "Male_11_Leg_Right" : "Female_11_Leg_Right", selector);
        DeactivateCategory(isMale ? "Female_12_Leg_Left" : "Male_12_Leg_Left");
        DeactivateCategory(isMale ? "Female_11_Leg_Right" : "Male_11_Leg_Right");
    }

    void ActivateHips(int selector)
    {
        ActivatePart(gender == Gender.Male ? "Male_10_Hips" : "Female_10_Hips", selector);
        DeactivateCategory(isMale ? "Female_10_Hips" : "Male_10_Hips");
    }

    void ActivateHand(int selector)
    {
        ActivatePart(gender == Gender.Male ? "Male_08_Hand_Right" : "Female_08_Hand_Right", selector);
        ActivatePart(gender == Gender.Male ? "Male_09_Hand_Left" : "Female_09_Hand_Left", selector);
        DeactivateCategory(isMale ? "Female_08_Hand_Right" : "Male_08_Hand_Right");
        DeactivateCategory(isMale ? "Female_09_Hand_Left" : "Male_09_Hand_Left");
    }

    void ActivateLowerArm(int selector)
    {
        ActivatePart(gender == Gender.Male ? "Male_06_Arm_Lower_Right" : "Female_06_Arm_Lower_Right", selector);
        ActivatePart(gender == Gender.Male ? "Male_07_Arm_Lower_Left" : "Female_07_Arm_Lower_Left", selector);
        DeactivateCategory(isMale ? "Female_06_Arm_Lower_Right" : "Male_06_Arm_Lower_Right");
        DeactivateCategory(isMale ? "Female_07_Arm_Lower_Left" : "Male_07_Arm_Lower_Left");
    }

    void ActivateUpperArm(int selector)
    {
        ActivatePart(isMale ? "Male_04_Arm_Upper_Right" : "Female_04_Arm_Upper_Right", selector);
        ActivatePart(isMale ? "Male_05_Arm_Upper_Left" : "Female_05_Arm_Upper_Left", selector);
        DeactivateCategory(isMale ? "Female_04_Arm_Upper_Right" : "Male_04_Arm_Upper_Right");
        DeactivateCategory(isMale ? "Female_05_Arm_Upper_Left" : "Male_05_Arm_Upper_Left");
    }

    void ActivateTorso(int selector)
    {
        ActivatePart(isMale ? "Male_03_Torso" : "Female_03_Torso", selector);
        DeactivateCategory(isMale ? "Female_03_Torso" : "Male_03_Torso");
    }

    void ActivateFacialHair(int selector)
    {
        if (!isMale)
        {
            DeactivateCategory("Male_02_FacialHair");
            return;
        }
        ActivatePart("Male_02_FacialHair", selector);
    }

    void ActivateEyebrows(int selector)
    {
        ActivatePart(isMale ? "Male_01_Eyebrows" : "Female_01_Eyebrows", selector);
        DeactivateCategory(isMale ? "Female_01_Eyebrows" : "Male_01_Eyebrows");
    }

    void ActivateHead(int selector)
    {
        ActivatePart(isMale ? "Male_Head_All_Elements" : "Female_Head_All_Elements", selector);
        DeactivateCategory(isMale ? "Female_Head_All_Elements" : "Male_Head_All_Elements");
    }

    public bool isMale => gender == Gender.Male;

    void ActivateHair(int selector)
    {
        ActivatePart("All_01_Hair", selector);
    }

    Transform Find(string n)
    {
        return GetComponentsInChildren<Transform>().First(x => x.name == n);
    }

    void ActivatePart(string identifier, int selector)
    {
        if (selector < 0)
        {
            DeactivateCategory(identifier);
            return;
        }
        if (!CharacterGameObjects.ContainsKey(identifier))
        {
            Debug.Log($"{identifier} not found in dictionary");
            return;
        }
        if ((CharacterGameObjects[identifier].Count < selector))
        {
            Debug.Log($"Index {selector }out of range for {identifier}");
            return;
        }
        DeactivateCategory(identifier);
        CharacterGameObjects[identifier][selector].SetActive(true);
    }

    void DeactivateCategory(string identifier)
    {
        if (!CharacterGameObjects.ContainsKey(identifier))
        {
            Debug.LogError($"Category {identifier} not found in database!");
            return;
        }
        foreach (GameObject g in CharacterGameObjects[identifier])
        {
            g.SetActive(false);
        }
    }

    public void InitGameObjects()
    {
        if (characterGameObjects != null) return;
        characterGameObjects = new Dictionary<string, List<GameObject>>();
        var catalogue = AllGenderBodyParts;
        BuildCharacterGameObjectFromCatalogue(AllGenderBodyParts);
        BuildCharacterGameObjectFromCatalogue(MaleBodyCategories);
        BuildCharacterGameObjectFromCatalogue(FemaleBodyCategories);
    }

    void BuildCharacterGameObjectFromCatalogue(string[] catalogue)
    {
        foreach (string category in catalogue)
        {
            Debug.Log($"Building {category}");
            List<GameObject> list = new List<GameObject>();
            Transform t = GetComponentsInChildren<Transform>().First(x => x.gameObject.name == category);
            if (t)
            {
                //foreach (Transform tr in t.GetComponentsInChildren<Transform>())
                for (int i = 0; i < t.childCount; i++)
                {
                    Transform tr = t.GetChild(i);
                    if (tr == t) continue;
                    {
                        list.Add(tr.gameObject);
                        tr.gameObject.SetActive(false);
                    }
                }

                characterGameObjects[category] = list;
            }
        }
    }

    public static void InitCharacterParts()
    {
        if (characterParts != null) return;
        GameObject character = Resources.Load<GameObject>("PolyFantasyHeroBase");
        if (character == null) Debug.Log("Unable to find Character!");
        characterParts = new Dictionary<string, List<string>>();
        BuildCategory(AllGenderBodyParts, character);
        BuildCategory(FemaleBodyCategories, character);
        BuildCategory(MaleBodyCategories, character);
        character = null;
    }

    static void BuildCategory(IEnumerable<string> parts, GameObject source)
    {
        foreach (string category in parts)
        {
            List<string> items = new List<string>();
            if (source == null)
            {
                Debug.Log("Source Not Loaded?");
            }
            else
            {
                Debug.Log($"Source is {source.name}");
            }
            Debug.Log($"Testing {category}");
            Transform t = source.GetComponentsInChildren<Transform>().First(x => x.gameObject.name == category);
            if (t == null)
            {
                Debug.Log($"Unable to locate {category}");
            }
            else
            {
                Debug.Log($"Category {t.name}");
            }
            foreach (Transform tr in t.gameObject.GetComponentsInChildren<Transform>())
            {
                if (tr == t) continue;
                GameObject go = tr.gameObject;
                Debug.Log($"Adding {go.name}");
                items.Add(go.name);
            }

            characterParts[category] = items;
            Debug.Log(characterParts[category].Count);
        }

    }
}