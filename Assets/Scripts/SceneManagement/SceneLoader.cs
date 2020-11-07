using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] GameObject ResumeButton;

    void Awake()
    {
        ResumeButton.SetActive(SavingSystem.SaveFileExists("Character"));
    }

    public void LoadScene(int scene)
    {
       
        SceneManager.LoadScene(scene);
    }

    public void LoadNewScene(int scene)
    {
        SavingSystem.Save("Character");
        SceneManager.LoadScene(scene);
    }
}
