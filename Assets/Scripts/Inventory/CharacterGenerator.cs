using System.Collections.Generic;
using System.Linq;
using GameDevTV.Inventories;
using PsychoticLab;
using TkrainDesigns.Saving;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour, ISaveable
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

    public static readonly string HeadCoverings_Base_Hair = "HeadCoverings_Base_Hair";
    public static readonly string HeadCoverings_No_FacialHair = "HeadCoverings_No_FacialHair";
    public static readonly string HeadCoverings_No_Hair = "HeadCoverings_No_Hair";
    public static readonly string All_01_Hair = "All_01_Hair";
    public static readonly string Helmet = "Helmet";
    public static readonly string Back_Attachment = "All_04_Back_Attachment";
    public static readonly string Shoulder_Attachment_Right = "All_05_Shoulder_Attachment_Right";
    public static readonly string Shoulder_Attachment_Left = "All_06_Shoulder_Attachment_Left";
    public static readonly string Elbow_Attachment_Right = "All_07_Elbow_Attachment_Right";
    public static readonly string Elbow_Attachment_Left = "All_08_Elbow_Attachment_Left";
    public static readonly string Hips_Attachment = "All_09_HipsAttachment";
    public static readonly string Knee_Attachment_Right = "All_10_Knee_Attachment_Right";
    public static readonly string Knee_Attachment_Left = "All_11_Knee_Atatchment_Left";
    public static readonly string Elf_Ear = "Elf_Ear";

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

    public static readonly string Female_Head_All_Elements = "Female_Head_All_Elements";
    public static readonly string Female_Head_NoElements = "Female_Head_NoElements";
    public static readonly string Female_Eyebrows = "Female_01_Eyebrows";
    public static readonly string Female_Torso = "Female_03_Torso";
    public static readonly string Female_Arm_Upper_Right = "Female_04_Arm_Upper_Right";
    public static readonly string Female_Arm_Upper_Left = "Female_05_Arm_Upper_Left";
    public static readonly string Female_Arm_Lower_Right = "Female_06_Arm_Lower_Right";
    public static readonly string Female_Arm_Lower_Left = "Female_07_Arm_Lower_Left";
    public static readonly string Female_Hand_Right = "Female_08_Hand_Right";
    public static readonly string Female_Hand_Left = "Female_09_Hand_Left";
    public static readonly string Female_Hips = "Female_10_Hips";
    public static readonly string Female_Leg_Right = "Female_11_Leg_Right";
    public static readonly string Female_Leg_Left = "Female_12_Leg_Left";

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

    public static readonly string Male_Head_All_Elements = "Male_Head_All_Elements";
    public static readonly string Male_Head_No_Elements = "Male_Head_No_Elements";
    public static readonly string Male_Eyebrows = "Male_01_Eyebrows";
    public static readonly string Male_FacialHair = "Male_02_FacialHair";
    public static readonly string Male_Torso = "Male_03_Torso";
    public static readonly string Male_Arm_Upper_Right = "Male_04_Arm_Upper_Right";
    public static readonly string Male_Arm_Upper_Left = "Male_05_Arm_Upper_Left";
    public static readonly string Male_Arm_Lower_Right = "Male_06_Arm_Lower_Right";
    public static readonly string Male_Arm_Lower_Left = "Male_07_Arm_Lower_Left";
    public static readonly string Male_Hand_Right = "Male_08_Hand_Right";
    public static readonly string Male_Hand_Left = "Male_09_Hand_Left";
    public static readonly string Male_Hips = "Male_10_Hips";
    public static readonly string Male_Leg_Right = "Male_11_Leg_Right";
    public static readonly string Male_Leg_Left = "Male_12_Leg_Left";

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

    public const string HairColor = "_Color_Hair";
    public const string SkinColor = "_Color_Skin";
    public const string PrimaryColor = "_Color_Primary";
    public const string SecondaryColor = "_Color_Secondary";
    public const string LeatherPrimaryColor = "_Color_Leather_Primary";
    public const string LeatherSecondaryColor = "_Color_Leather_Secondary";
    public const string MetalPrimaryColor = "_Color_Metal_Primary";
    public const string MetalSecondaryColor = "_Color_Metal_Secondary";
    public const string MetalDarkColor = "_Color_Metal_Dark";

    public static string[] GearColors = new string[]
                                        {
                                            PrimaryColor,
                                            SecondaryColor,
                                            LeatherPrimaryColor,
                                            LeatherSecondaryColor,
                                            MetalPrimaryColor,
                                            MetalSecondaryColor,
                                            MetalDarkColor
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

    public static Color[] primary =
    {
        new Color(0.2862745f, 0.4f, 0.4941177f), new Color(0.4392157f, 0.1960784f, 0.172549f),
        new Color(0.3529412f, 0.3803922f, 0.2705882f), new Color(0.682353f, 0.4392157f, 0.2196079f),
        new Color(0.4313726f, 0.2313726f, 0.2705882f), new Color(0.5921569f, 0.4941177f, 0.2588235f),
        new Color(0.482353f, 0.4156863f, 0.3529412f), new Color(0.2352941f, 0.2352941f, 0.2352941f),
        new Color(0.2313726f, 0.4313726f, 0.4156863f), Color.magenta, Color.blue, Color.green, Color.red,
        new Color32(165, 42, 42, 255), new Color32(255, 165, 0, 255)
    };

    public static Color[] secondary =
    {
        new Color(0.7019608f, 0.6235294f, 0.4666667f), new Color(0.7372549f, 0.7372549f, 0.7372549f),
        new Color(0.1647059f, 0.1647059f, 0.1647059f), new Color(0.2392157f, 0.2509804f, 0.1882353f), Color.magenta,
        Color.blue, Color.green, Color.red, new Color32(165, 42, 42, 255), new Color32(255, 165, 0, 255)
    };

    public static Color[] metalPrimary =
    {
        new Color(0.6705883f, 0.6705883f, 0.6705883f), new Color(0.5568628f, 0.5960785f, 0.6392157f),
        new Color(0.5568628f, 0.6235294f, 0.6f), new Color(0.6313726f, 0.6196079f, 0.5568628f),
        new Color(0.6980392f, 0.6509804f, 0.6196079f), Color.magenta, Color.blue, Color.green, Color.red,
        new Color32(165, 42, 42, 255), new Color32(255, 165, 0, 255)
    };

    public static Color[] metalSecondary =
    {
        new Color(0.3921569f, 0.4039216f, 0.4117647f), new Color(0.4784314f, 0.5176471f, 0.5450981f),
        new Color(0.3764706f, 0.3607843f, 0.3372549f), new Color(0.3254902f, 0.3764706f, 0.3372549f),
        new Color(0.4f, 0.4039216f, 0.3568628f), Color.magenta, Color.blue, Color.green, Color.red,
        new Color32(165, 42, 42, 255), new Color32(255, 165, 0, 255)
    };

    public static Color[] metalDark =
    {
        new Color32(0x2D, 0x32, 0x37, 0xff), new Color32(0x1D, 0x22, 0x27, 0xff),
        new Color32(0x50, 0x50, 0x50, 0xff), new Color32(0x90, 0x90, 0x90, 0xff),
        new Color32(0x0c, 0x37, 0x63, 0xff), new Color32(0x63, 0x37, 0x0c, 0xff),
        new Color32(0x0c, 0x63, 0x37, 0xff), Color.magenta, Color.blue, Color.green, Color.red,
        new Color32(165, 42, 42, 255), new Color32(255, 165, 0, 255)
    };

    public static Color[] leatherPrimary =
    {
        new Color(0.282353f, 0.2078432f, 0.1647059f), new Color(0.342549f, 0.3094118f, 0.2384314f),
        new Color32(0x4D, 0x2C, 0x18, 0xFF), Color.magenta, Color.blue, Color.green, Color.red,
        new Color32(165, 42, 42, 255), new Color32(255, 165, 0, 255)
    };

    public static Color[] leatherSecondary =
    {
        new Color(0.372549f, 0.3294118f, 0.2784314f), new Color(0.5566038f, 0.4759794f, 0.4759794f), Color.magenta,
        Color.blue, Color.green, Color.red, new Color32(165, 42, 42, 255), new Color32(255, 165, 0, 255)
    };

    public static Color[] skinColors =
    {
        new Color(1f, 0.8000001f, 0.682353f), new Color(0.8196079f, 0.6352941f, 0.4588236f),
        new Color(0.5647059f, 0.4078432f, 0.3137255f)
    };

    public static Color[] hairColors =
    {
        new Color(0.3098039f, 0.254902f, 0.1764706f), new Color(0.1764706f, 0.1686275f, 0.1686275f),
        new Color(0.8313726f, 0.6235294f, 0.3607843f), new Color(0.9339623f, 0.3688644f, 0.06608222f), Color.magenta,
        Color.blue, Color.green, Color.red
    };

    public static Color whiteScar = new Color(0.9294118f, 0.6862745f, 0.5921569f);
    public static Color brownScar = new Color(0.6980392f, 0.5450981f, 0.4f);
    public static Color blackScar = new Color(0.4235294f, 0.3176471f, 0.282353f);
    public static Color elfScar = new Color(0.8745099f, 0.6588235f, 0.6313726f);

    public static Color[] bodyArt =
    {
        new Color(0.0509804f, 0.6745098f, 0.9843138f), new Color(0.7215686f, 0.2666667f, 0.2666667f),
        new Color(0.3058824f, 0.7215686f, 0.6862745f), new Color(0.9254903f, 0.882353f, 0.8509805f),
        new Color(0.3098039f, 0.7058824f, 0.3137255f), new Color(0.5294118f, 0.3098039f, 0.6470588f),
        new Color(0.8666667f, 0.7764707f, 0.254902f), new Color(0.2392157f, 0.4588236f, 0.8156863f)
    };

    int hairColor = 0;
    int skinColor = 0;

    Equipment equipment;

    void Awake()
    {
        gender = PlayerPrefs.GetInt("Gender") == 1 ? Gender.Male : Gender.Female;
        equipment = GetComponent<Equipment>();
        equipment.EquipmentUpdated += LoadArmor;
        LoadDefaultCharacter();
    }

    public void SetGender(bool female)
    {
        gender = female ? Gender.Female : Gender.Male;
        LoadDefaultCharacter();
    }

    public void SetHairColor(int index)
    {
        if (index >= 0 && index < hairColors.Length)
        {
            hairColor = index;
        }

        SetColorInCategory(All_01_Hair, HairColor, hairColors[hairColor]);
        SetColorInCategory(Male_FacialHair, HairColor, hairColors[hairColor]);
        SetColorInCategory(Female_Eyebrows, HairColor, hairColors[hairColor]);
        SetColorInCategory(Male_Eyebrows, HairColor, hairColors[hairColor]);
    }

    public void CycleHairColor(int index)
    {
        hairColor += index;
        if (hairColor < 0) hairColor = hairColors.Length - 1;
        hairColor = hairColor % hairColors.Length;
        SetHairColor(hairColor);
    }

    public void CycleSkinColor(int index)
    {
        skinColor += index;
        if (skinColor < 0) skinColor += skinColors.Length-1;
        skinColor = skinColor % skinColors.Length;
        SetSkinColor(skinColor);
    }

    public void CycleHairStyle(int index)
    {
        hair += index;
        if (hair < -1) hair = characterGameObjects[All_01_Hair].Count - 1;
        hair %= CharacterGameObjects[All_01_Hair].Count;
        ActivateHair(hair);
    }

    public void CycleFacialHair(int index)
    {
        facialHair += index;
        int maxHair = CharacterGameObjects[Male_FacialHair].Count;
        if (facialHair < -1) facialHair = maxHair - 1;
        if (facialHair >= maxHair) facialHair = -1;
        ActivateFacialHair(facialHair);
    }

    public void CycleHead(int index)
    {
        head += index;
        if (head < 0) head += CharacterGameObjects[Female_Head_All_Elements].Count-1;
        head %= CharacterGameObjects[Female_Head_All_Elements].Count;
        ActivateHead(head);
    }

    public void CycleEyebrows(int index)
    {
        eyebrow += index;
        if (eyebrow < 0) eyebrow += CharacterGameObjects[Female_Eyebrows].Count-1;
        eyebrow %= CharacterGameObjects[Female_Eyebrows].Count;
        ActivateEyebrows(eyebrow);
    }

    void SetColorInCategory(string category, string shaderVariable, Color colorToSet)
    {
        if (!CharacterGameObjects.ContainsKey(category)) return;
        //Debug.Log($"Setting {shaderVariable} on {category}");
        foreach (GameObject go in CharacterGameObjects[category])
        {
            Renderer rend = go.GetComponent<Renderer>();
            rend.material.SetColor(shaderVariable, colorToSet);
        }
    }

    public void SetSkinColor(int index)
    {
        if (index >= 0 && index < skinColors.Length)
        {
            skinColor = index;
        }

        foreach (var pair in CharacterGameObjects)
        {
            SetColorInCategory(pair.Key, "_Color_Skin", skinColors[skinColor]);
        }
    }

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
        ActivatePart(gender == Gender.Male ? "Male_12_Leg_Left" : "Female_12_Leg_Left", selector, colorChanges);
        ActivatePart(gender == Gender.Male ? "Male_11_Leg_Right" : "Female_11_Leg_Right", selector, colorChanges);
        DeactivateCategory(isMale ? "Female_12_Leg_Left" : "Male_12_Leg_Left");
        DeactivateCategory(isMale ? "Female_11_Leg_Right" : "Male_11_Leg_Right");
    }

    void ActivateHips(int selector, List<ItemPair> colorChanges = null)
    {
        ActivatePart(gender == Gender.Male ? "Male_10_Hips" : "Female_10_Hips", selector, colorChanges);
        DeactivateCategory(isMale ? "Female_10_Hips" : "Male_10_Hips");
    }

    void ActivateHand(int selector, List<ItemPair> colorChanges = null)
    {
        ActivatePart(gender == Gender.Male ? "Male_08_Hand_Right" : "Female_08_Hand_Right", selector, colorChanges);
        ActivatePart(gender == Gender.Male ? "Male_09_Hand_Left" : "Female_09_Hand_Left", selector, colorChanges);
        DeactivateCategory(isMale ? "Female_08_Hand_Right" : "Male_08_Hand_Right");
        DeactivateCategory(isMale ? "Female_09_Hand_Left" : "Male_09_Hand_Left");
    }

    void ActivateLowerArm(int selector, List<ItemPair> colorChanges = null)
    {
        ActivatePart(gender == Gender.Male ? "Male_06_Arm_Lower_Right" : "Female_06_Arm_Lower_Right", selector,
                     colorChanges);
        ActivatePart(gender == Gender.Male ? "Male_07_Arm_Lower_Left" : "Female_07_Arm_Lower_Left", selector,
                     colorChanges);
        DeactivateCategory(isMale ? "Female_06_Arm_Lower_Right" : "Male_06_Arm_Lower_Right");
        DeactivateCategory(isMale ? "Female_07_Arm_Lower_Left" : "Male_07_Arm_Lower_Left");
    }

    void ActivateUpperArm(int selector, List<ItemPair> colorChanges = null)
    {
        ActivatePart(isMale ? "Male_04_Arm_Upper_Right" : "Female_04_Arm_Upper_Right", selector, colorChanges);
        ActivatePart(isMale ? "Male_05_Arm_Upper_Left" : "Female_05_Arm_Upper_Left", selector, colorChanges);
        DeactivateCategory(isMale ? "Female_04_Arm_Upper_Right" : "Male_04_Arm_Upper_Right");
        DeactivateCategory(isMale ? "Female_05_Arm_Upper_Left" : "Male_05_Arm_Upper_Left");
    }

    void ActivateTorso(int selector, List<ItemPair> colorChanges = null)
    {
        ActivatePart(isMale ? "Male_03_Torso" : "Female_03_Torso", selector, colorChanges);
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
            //Debug.Log($"Building {category}");
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
        gender = bundle.GetInt("Gender", 1) == 1 ? Gender.Male : Gender.Female;
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
                value = CycleValue(Male_Torso, value, i);
                ActivateTorso(value);
                break;
            case "UpperArm":
                value = CycleValue(Male_Arm_Upper_Left, value, i);
                ActivateUpperArm(value);
                break;
            case "LowerArm":
                value = CycleValue(Male_Arm_Lower_Left, value, i);
                ActivateLowerArm(value);
                break;
            case "Hand":
                value = CycleValue(Male_Hand_Left, value, i);
                ActivateHand(value);
                break;
            case "Hips":
                value = CycleValue(Male_Hips, value, i);
                ActivateHips(value);
                break;
            case "Leg":
                value = CycleValue(Male_Leg_Left, value, i);
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
        colorToSet = GetColor(parameterString, value);
        //Debug.Log($"Changing {item}/{parameterString} to {colorToSet}");
        item.GetComponent<Renderer>().material.SetColor(parameterString, colorToSet);
    }

    public static Color GetColor(string parameterString, int value)
    {
        Color colorToSet;
        switch (parameterString)
        {
            case LeatherPrimaryColor:
                colorToSet = leatherPrimary[value];
                break;
            case LeatherSecondaryColor:
                colorToSet = leatherSecondary[value];
                break;
            case MetalPrimaryColor:
                colorToSet = metalPrimary[value];
                break;
            case MetalSecondaryColor:
                colorToSet = metalSecondary[value];
                break;
            case PrimaryColor:
                colorToSet = primary[value];
                break;
            case SecondaryColor:
                colorToSet = secondary[value];
                break;
            case MetalDarkColor:
                colorToSet = metalDark[value];
                break;
            default:
                colorToSet = Color.white;
                break;
        }

        return colorToSet;
    }


    public int CycleColor(string parameterString, int value, int modifier)
    {
        int cycleValue = 0;
        cycleValue = GetColorCount(parameterString);

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

    public static int GetColorCount(string parameterString)
    {
        int cycleValue;
        switch (parameterString)
        {
            case LeatherPrimaryColor:
                cycleValue = leatherPrimary.Length;
                break;
            case LeatherSecondaryColor:
                cycleValue = leatherSecondary.Length;
                break;
            case MetalPrimaryColor:
                cycleValue = metalPrimary.Length;
                break;
            case MetalSecondaryColor:
                cycleValue = metalSecondary.Length;
                break;
            case PrimaryColor:
                cycleValue = primary.Length;
                break;
            case SecondaryColor:
                cycleValue = secondary.Length;
                break;
            case MetalDarkColor:
                cycleValue = metalDark.Length;
                break;
            default:
                cycleValue = 0;
                break;
        }

        return cycleValue;
    }
}