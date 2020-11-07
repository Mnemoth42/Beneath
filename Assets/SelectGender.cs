using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGender : MonoBehaviour
{
    public Image maleSelected;
    public Image femaleSelected;
    public Image maleUnselected;
    public Image femaleUnselected;

    public bool male = true;

    void Awake()
    {
        male = PlayerPrefs.GetInt("Gender") == 1;
        UpdateDisplay();
    }

    public void SetMale()
    {
        male = true;
        PlayerPrefs.SetInt("Gender", 1);
        UpdateDisplay();
    }

    public void SetFemale()
    {
        male = false;
        PlayerPrefs.SetInt("Gender", 0);
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        maleSelected.enabled = male;
        femaleUnselected.enabled = male;
        femaleSelected.enabled = !male;
        maleUnselected.enabled = !male;
    }
}
