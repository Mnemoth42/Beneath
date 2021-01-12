using System.Collections.Generic;
using System.Linq;
using GameDevTV.Inventories;
using TkrainDesigns.Saving;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour, ISaveable
{
    

    #region statics

    #endregion

    #region Fields

    /// <summary>
    /// A dictionary containing all of the modular parts, organized by category.
    /// Use this catalogue, not the static one when actually customizing the character.
    /// </summary>
    Dictionary<string, List<GameObject>> characterGameObjects;
    /// <summary>
    /// A dictionary containing all of the modular parts, organized by category.
    /// Use this catalogue, not the static one when actually customizing the character.
    /// </summary>
    Dictionary<string, List<GameObject>> CharacterGameObjects
    {
        get
        {
            InitGameObjects(); //This will build the dictionary if it hasn't yet been initialized.
            return characterGameObjects;
        }
    }

    [SerializeField] SyntyStatics.Gender gender = SyntyStatics.Gender.Male;
    [SerializeField] SyntyStatics.Race race = SyntyStatics.Race.Human;
    [Range(0, 37)] [SerializeField] int hair = 0;
    [Range(0, 21)] [SerializeField] int head = 0;
    [Range(0, 6)] [SerializeField] int eyebrow = 0;
    [Range(0, 17)] [SerializeField] int facialHair = 0;
    [Range(0, 27)] [SerializeField] int defaultTorso = 1;
    [Range(0, 20)] [SerializeField] int defaultUpperArm = 0;
    [Range(0, 17)] [SerializeField] int defaultLowerArm = 0;
    [Range(0, 16)] [SerializeField] int defaultHand = 0;
    [Range(0, 27)] [SerializeField] int defaultHips = 0;
    [Range(0, 18)] [SerializeField] int defaultLeg = 0;
    public bool isMale => gender == SyntyStatics.Gender.Male;

    

    int hairColor = 0;
    int skinColor = 0;

    Equipment equipment;

    #endregion

    #region Initialization

    void Awake()
    {
        gender = PlayerPrefs.GetInt("Gender") == 1 ? SyntyStatics.Gender.Male : SyntyStatics.Gender.Female;
        equipment = GetComponent<Equipment>();
        equipment.EquipmentUpdated += LoadArmor;
        LoadDefaultCharacter();
    }

    public void InitGameObjects()
    {
        if (characterGameObjects != null) return;
        characterGameObjects = new Dictionary<string, List<GameObject>>();
        BuildCharacterGameObjectFromCatalogue(SyntyStatics.AllGenderBodyParts);
        BuildCharacterGameObjectFromCatalogue(SyntyStatics.MaleBodyCategories);
        BuildCharacterGameObjectFromCatalogue(SyntyStatics.FemaleBodyCategories);
    }

    void BuildCharacterGameObjectFromCatalogue(string[] catalogue)
    {
        foreach (string category in catalogue)
        {
            List<GameObject> list = new List<GameObject>();
            Transform t = GetComponentsInChildren<Transform>().First(x => x.gameObject.name == category);
            if (t)
            {
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

    #endregion

    #region Character Generation

    /// <summary>
    /// Should only be called when creating the character or from within RestoreState()
    /// </summary>
    /// <param name="female"></param>
    public void SetGender(bool female)
    {
        gender = female ? SyntyStatics.Gender.Female : SyntyStatics.Gender.Male;
        LoadDefaultCharacter();
    }

    /// <summary>
    /// Should only be called when creating the character or from within RestoreState()
    /// </summary>
    /// <param name="index"></param>
    public void SetHairColor(int index)
    {
        if (index >= 0 && index < SyntyStatics.hairColors.Length)
        {
            hairColor = index;
        }

        SetColorInCategory(SyntyStatics.All_01_Hair, SyntyStatics.HairColor, SyntyStatics.hairColors[hairColor]);
        SetColorInCategory(SyntyStatics.Male_FacialHair, SyntyStatics.HairColor, SyntyStatics.hairColors[hairColor]);
        SetColorInCategory(SyntyStatics.Female_Eyebrows, SyntyStatics.HairColor, SyntyStatics.hairColors[hairColor]);
        SetColorInCategory(SyntyStatics.Male_Eyebrows, SyntyStatics.HairColor, SyntyStatics.hairColors[hairColor]);
    }
    /// <summary>
    /// Should only be called when creating the character.
    /// </summary>
    /// <param name="index"></param>
    public void CycleHairColor(int index)
    {
        hairColor += index;
        if (hairColor < 0) hairColor = SyntyStatics.hairColors.Length - 1;
        hairColor = hairColor % SyntyStatics.hairColors.Length;
        SetHairColor(hairColor);
    }
    /// <summary>
    /// Should only be called when creating the character.
    /// </summary>
    /// <param name="index"></param>
    public void CycleSkinColor(int index)
    {
        skinColor += index;
        if (skinColor < 0) skinColor += SyntyStatics.skinColors.Length-1;
        skinColor = skinColor % SyntyStatics.skinColors.Length;
        SetSkinColor(skinColor);
    }
    /// <summary>
    /// Should only be called when creating the character.
    /// </summary>
    /// <param name="index"></param>
    public void CycleHairStyle(int index)
    {
        hair += index;
        if (hair < -1) hair = characterGameObjects[SyntyStatics.All_01_Hair].Count - 1;
        hair %= CharacterGameObjects[SyntyStatics.All_01_Hair].Count;
        ActivateHair(hair);
    }
    /// <summary>
    /// Should only be called when creating the character.
    /// </summary>
    /// <param name="index"></param>
    public void CycleFacialHair(int index)
    {
        facialHair += index;
        int maxHair = CharacterGameObjects[SyntyStatics.Male_FacialHair].Count;
        if (facialHair < -1) facialHair = maxHair - 1;
        if (facialHair >= maxHair) facialHair = -1;
        ActivateFacialHair(facialHair);
    }
    /// <summary>
    /// Should only be called when creating the character.
    /// </summary>
    /// <param name="index"></param>
    public void CycleHead(int index)
    {
        head += index;
        if (head < 0) head += CharacterGameObjects[SyntyStatics.Female_Head_All_Elements].Count-1;
        head %= CharacterGameObjects[SyntyStatics.Female_Head_All_Elements].Count;
        ActivateHead(head);
    }
    /// <summary>
    /// Should only be called when creating the character.
    /// </summary>
    /// <param name="index"></param>
    public void CycleEyebrows(int index)
    {
        eyebrow += index;
        if (eyebrow < 0) eyebrow += CharacterGameObjects[SyntyStatics.Female_Eyebrows].Count-1;
        eyebrow %= CharacterGameObjects[SyntyStatics.Female_Eyebrows].Count;
        ActivateEyebrows(eyebrow);
    }
    /// <summary>
    /// Should only be called when creating the character.
    /// </summary>
    /// <param name="category"></param>
    /// <param name="shaderVariable"></param>
    /// <param name="colorToSet"></param>
    void SetColorInCategory(string category, string shaderVariable, Color colorToSet)
    {
        if (!CharacterGameObjects.ContainsKey(category)) return;
        foreach (GameObject go in CharacterGameObjects[category])
        {
            Renderer rend = go.GetComponent<Renderer>();
            rend.material.SetColor(shaderVariable, colorToSet);
        }
    }
    /// <summary>
    /// Should only be called when creating the character or from RestoreState
    /// </summary>
    /// <param name="index"></param>
    public void SetSkinColor(int index)
    {
        if (index >= 0 && index < SyntyStatics.skinColors.Length)
        {
            skinColor = index;
        }

        foreach (var pair in CharacterGameObjects)
        {
            SetColorInCategory(pair.Key, "_Color_Skin", SyntyStatics.skinColors[skinColor]);
        }
    }

    #endregion

    #region CharacterActivation

    /// <summary>
    /// This sets the character to the default state, assuming no items in the EquipmentManager. 
    /// </summary>
    public void LoadDefaultCharacter()
    {
        foreach (var pair in CharacterGameObjects)
        {
            foreach (var item in pair.Value)
            {
                item.SetActive(false);
            }
        }

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
           // Debug.Log(pair.Key.GetDisplayName());
            foreach (string category in pair.Value.SlotsToDeactivate)
            {
                DeactivateCategory(category);
            }

            var colorChanger = pair.Value.ColorChangers;
            foreach (ItemPair itemPair in pair.Value.ObjectsToActivate)
            {
                //Debug.Log($"{itemPair.category}-{itemPair.index}");
                switch (itemPair.category)
                {
                    case "Leg":
                        ActivateLeg(itemPair.index, colorChanger);
                        break;
                    case "Hips":
                        ActivateHips(itemPair.index,colorChanger);
                        break;
                    case "Torso":
                        ActivateTorso(itemPair.index,colorChanger);
                        break;
                    case "UpperArm":
                        ActivateUpperArm(itemPair.index,colorChanger);
                        break;
                    case "LowerArm":
                        ActivateLowerArm(itemPair.index,colorChanger);
                        break;
                    case "Hand":
                        ActivateHand(itemPair.index,colorChanger);
                        break;
                    default:
                        ActivatePart(itemPair.category, itemPair.index, colorChanger);
                        break;
                }
            }
        }
    }


    void ActivateLeg(int selector, List<ItemPair> colorChanges = null)
    {
        ActivatePart(gender == SyntyStatics.Gender.Male ? SyntyStatics.Male_Leg_Left : SyntyStatics.Female_Leg_Left, selector, colorChanges);
        ActivatePart(gender == SyntyStatics.Gender.Male ? SyntyStatics.Male_Leg_Right : SyntyStatics.Female_Leg_Right, selector, colorChanges);
        DeactivateCategory(isMale ? SyntyStatics.Female_Leg_Left : SyntyStatics.Male_Leg_Left);
        DeactivateCategory(isMale ? SyntyStatics.Female_Leg_Right : SyntyStatics.Male_Leg_Right);
    }

    void ActivateHips(int selector, List<ItemPair> colorChanges = null)
    {
        ActivatePart(gender == SyntyStatics.Gender.Male ? SyntyStatics.Male_Hips : SyntyStatics.Female_Hips, selector, colorChanges);
        DeactivateCategory(isMale ? SyntyStatics.Female_Hips : SyntyStatics.Male_Hips);
    }

    void ActivateHand(int selector, List<ItemPair> colorChanges = null)
    {
        ActivatePart(gender == SyntyStatics.Gender.Male ? SyntyStatics.Male_Hand_Right : SyntyStatics.Female_Hand_Right, selector, colorChanges);
        ActivatePart(gender == SyntyStatics.Gender.Male ? SyntyStatics.Male_Hand_Left : SyntyStatics.Female_Hand_Left, selector, colorChanges);
        DeactivateCategory(isMale ? SyntyStatics.Female_Hand_Right : SyntyStatics.Male_Hand_Right);
        DeactivateCategory(isMale ? SyntyStatics.Female_Hand_Left : SyntyStatics.Male_Hand_Left);
    }

    void ActivateLowerArm(int selector, List<ItemPair> colorChanges = null)
    {
        ActivatePart(gender == SyntyStatics.Gender.Male ? SyntyStatics.Male_Arm_Lower_Right : SyntyStatics.Female_Arm_Lower_Right, selector,
                     colorChanges);
        ActivatePart(gender == SyntyStatics.Gender.Male ? SyntyStatics.Male_Arm_Lower_Left : SyntyStatics.Female_Arm_Lower_Left, selector,
                     colorChanges);
        DeactivateCategory(isMale ? SyntyStatics.Female_Arm_Lower_Right : SyntyStatics.Male_Arm_Lower_Right);
        DeactivateCategory(isMale ? SyntyStatics.Female_Arm_Lower_Left : SyntyStatics.Male_Arm_Lower_Left);
    }

    void ActivateUpperArm(int selector, List<ItemPair> colorChanges = null)
    {
        ActivatePart(isMale ? SyntyStatics.Male_Arm_Upper_Right : SyntyStatics.Female_Arm_Upper_Right, selector, colorChanges);
        ActivatePart(isMale ? SyntyStatics.Male_Arm_Upper_Left : SyntyStatics.Female_Arm_Upper_Left, selector, colorChanges);
        DeactivateCategory(isMale ? SyntyStatics.Female_Arm_Upper_Right : SyntyStatics.Male_Arm_Upper_Right);
        DeactivateCategory(isMale ? SyntyStatics.Female_Arm_Upper_Left : SyntyStatics.Male_Arm_Upper_Left);
    }

    void ActivateTorso(int selector, List<ItemPair> colorChanges = null)
    {
        ActivatePart(isMale ? SyntyStatics.Male_Torso : SyntyStatics.Female_Torso, selector, colorChanges);
        DeactivateCategory(isMale ? SyntyStatics.Female_Torso : SyntyStatics.Male_Torso);
    }

    void ActivateFacialHair(int selector)
    {
        if (!isMale)
        {
            DeactivateCategory(SyntyStatics.Male_FacialHair);
            return;
        }

        ActivatePart(SyntyStatics.Male_FacialHair, selector);
    }

    void ActivateEyebrows(int selector)
    {
        ActivatePart(isMale ? SyntyStatics.Male_Eyebrows : SyntyStatics.Female_Eyebrows, selector);
        DeactivateCategory(isMale ? SyntyStatics.Female_Eyebrows : SyntyStatics.Male_Eyebrows);
    }

    void ActivateHead(int selector)
    {
        ActivatePart(isMale ? SyntyStatics.Male_Head_All_Elements : SyntyStatics.Female_Head_All_Elements, selector);
        DeactivateCategory(isMale ? SyntyStatics.Female_Head_All_Elements : SyntyStatics.Male_Head_All_Elements);
    }



    void ActivateHair(int selector)
    {
        ActivatePart(SyntyStatics.All_01_Hair, selector);
    }


    void ActivatePart(string identifier, int selector, List<ItemPair> colorChanges = null)
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
            Debug.Log($"Index {selector}out of range for {identifier}");
            return;
        }

        DeactivateCategory(identifier);
        GameObject go = CharacterGameObjects[identifier][selector];
        go.SetActive(true);
        if (colorChanges == null) return;
        foreach (var pair in colorChanges)
        { 
            SetColor(go, pair.category, pair.index);
        }
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

    #endregion

    #region StaticDictionary

    /// <summary>
    /// This static dictionary is for a hook for the custom editors for EquipableItem and other Editor windows.
    /// Outside of this, it should not be used as a reference to get/set items on the character because it is
    /// terribly inefficient for this purpose.
    /// </summary>
    static Dictionary<string, List<string>> characterParts;

    public static Dictionary<string, List<string>> CharacterParts
    {
        get
        {
            InitCharacterParts();
            return characterParts;
        }
    }

    public static void InitCharacterParts()
    {
        if (characterParts != null) return;
        GameObject character = Resources.Load<GameObject>("PolyFantasyHeroBase");
        if (character == null) Debug.Log("Unable to find Character!");
        characterParts = new Dictionary<string, List<string>>();
        BuildCategory(SyntyStatics.AllGenderBodyParts, character);
        BuildCategory(SyntyStatics.FemaleBodyCategories, character);
        BuildCategory(SyntyStatics.MaleBodyCategories, character);
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
    #endregion

    #region ISaveable

    public SaveBundle CaptureState()
    {
        SaveBundle bundle = new SaveBundle();
        bundle.PutInt("Gender", isMale ? 1 : 0);
        bundle.PutInt("Hair", hair);
        bundle.PutInt("FacialHair", facialHair);
        bundle.PutInt("Head", head);
        bundle.PutInt("Eyebrows", eyebrow);
        bundle.PutInt("SkinColor", skinColor);
        bundle.PutInt("HairColor", hairColor);
        return bundle;
    }


    public void RestoreState(SaveBundle bundle)
    {
        equipment.EquipmentUpdated -= LoadArmor; //prevent issues
        gender = bundle.GetInt("Gender", 1) == 1 ? SyntyStatics.Gender.Male : SyntyStatics.Gender.Female;
        hair = bundle.GetInt("Hair", hair);
        facialHair = bundle.GetInt("FacialHair", facialHair);
        head = bundle.GetInt("Head", head);
        eyebrow = bundle.GetInt("Eyebrows", eyebrow);
        skinColor = bundle.GetInt("skinColor", skinColor);
        hairColor = bundle.GetInt("HairColor", hairColor);
        SetHairColor(hairColor);
        SetSkinColor(skinColor);
        equipment.EquipmentUpdated += LoadArmor;
        Invoke(nameof(LoadArmor), .1f);
    }

    #endregion

    /* This section is used by the EquipmentBuilder scene only */
    #region EquipmentBuilder

    public int SetParameter(string parameterString, int value, int i)
    {
        if (!CharacterGameObjects.ContainsKey(parameterString)) return TryAlternateParameter(parameterString, value, i);
        int available = CharacterGameObjects[parameterString].Count;
        value += i;
        if (value >= available) value = -1;
        else if (value < -1) value = available - 1;
        ActivatePart(parameterString, value);
        return value;
    }

    int TryAlternateParameter(string parameterString, int value, int i)
    {
        switch (parameterString)
        {
            case "Torso":
                value = CycleValue(SyntyStatics.Male_Torso, value, i);
                ActivateTorso(value);
                break;
            case "UpperArm":
                value = CycleValue(SyntyStatics.Male_Arm_Upper_Left, value, i);
                ActivateUpperArm(value);
                break;
            case "LowerArm":
                value = CycleValue(SyntyStatics.Male_Arm_Lower_Left, value, i);
                ActivateLowerArm(value);
                break;
            case "Hand":
                value = CycleValue(SyntyStatics.Male_Hand_Left, value, i);
                ActivateHand(value);
                break;
            case "Hips":
                value = CycleValue(SyntyStatics.Male_Hips, value, i);
                ActivateHips(value);
                break;
            case "Leg":
                value = CycleValue(SyntyStatics.Male_Leg_Left, value, i);
                ActivateLeg(value);
                break;
            default:
                value = -999;
                break;
        }

        return value;
    }

    int CycleValue(string parameterString, int value, int i)
    {
        int available = CharacterGameObjects[parameterString].Count;
        value += i + available;
        value %= available;
        return value;
    }

    void SetColor(GameObject item, string parameterString, int value)
    {
        
        Color colorToSet = Color.white;
        colorToSet = SyntyStatics.GetColor(parameterString, value);
        //Debug.Log($"Changing {item}/{parameterString} to {colorToSet}");
        item.GetComponent<Renderer>().material.SetColor(parameterString, colorToSet);
    }


    public int CycleColor(string parameterString, int value, int modifier)
    {
        int cycleValue = 0;
        cycleValue = SyntyStatics.GetColorCount(parameterString);

        value += modifier + cycleValue;
        value %= cycleValue;

        foreach (var itemList in CharacterGameObjects.Values)
        {
            foreach (GameObject item in itemList)
            {
                SetColor(item, parameterString, value);
            }
        }

        return value;
    }

    #endregion
}