using UnityEngine;

public static class SyntyStatics
{
    public const string HairColor = "_Color_Hair";
    public const string SkinColor = "_Color_Skin";
    public const string PrimaryColor = "_Color_Primary";
    public const string SecondaryColor = "_Color_Secondary";
    public const string LeatherPrimaryColor = "_Color_Leather_Primary";
    public const string LeatherSecondaryColor = "_Color_Leather_Secondary";
    public const string MetalPrimaryColor = "_Color_Metal_Primary";
    public const string MetalSecondaryColor = "_Color_Metal_Secondary";
    public const string MetalDarkColor = "_Color_Metal_Dark";
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

    public enum Gender { Male, Female }

    public enum Race { Human, Elf }

    public static Color GetColor(string parameterString, int value)
    {
        Color colorToSet;
        switch (parameterString)
        {
            case SyntyStatics.LeatherPrimaryColor:
                colorToSet = SyntyStatics.leatherPrimary[value];
                break;
            case SyntyStatics.LeatherSecondaryColor:
                colorToSet = SyntyStatics.leatherSecondary[value];
                break;
            case SyntyStatics.MetalPrimaryColor:
                colorToSet = SyntyStatics.metalPrimary[value];
                break;
            case SyntyStatics.MetalSecondaryColor:
                colorToSet = SyntyStatics.metalSecondary[value];
                break;
            case SyntyStatics.PrimaryColor:
                colorToSet = SyntyStatics.primary[value];
                break;
            case SyntyStatics.SecondaryColor:
                colorToSet = SyntyStatics.secondary[value];
                break;
            case SyntyStatics.MetalDarkColor:
                colorToSet = SyntyStatics.metalDark[value];
                break;
            default:
                colorToSet = Color.white;
                break;
        }

        return colorToSet;
    }

    public static int GetColorCount(string parameterString)
    {
        int cycleValue;
        switch (parameterString)
        {
            case SyntyStatics.LeatherPrimaryColor:
                cycleValue = SyntyStatics.leatherPrimary.Length;
                break;
            case SyntyStatics.LeatherSecondaryColor:
                cycleValue = SyntyStatics.leatherSecondary.Length;
                break;
            case SyntyStatics.MetalPrimaryColor:
                cycleValue = SyntyStatics.metalPrimary.Length;
                break;
            case SyntyStatics.MetalSecondaryColor:
                cycleValue = SyntyStatics.metalSecondary.Length;
                break;
            case SyntyStatics.PrimaryColor:
                cycleValue = SyntyStatics.primary.Length;
                break;
            case SyntyStatics.SecondaryColor:
                cycleValue = SyntyStatics.secondary.Length;
                break;
            case SyntyStatics.MetalDarkColor:
                cycleValue = SyntyStatics.metalDark.Length;
                break;
            default:
                cycleValue = 0;
                break;
        }

        return cycleValue;
    }
}