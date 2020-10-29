using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        
    }

    void Start()
    {
        Open();
    }

    public void Open()
    {
        anim.SetTrigger("openLid");
    }

    public void Close()
    {
        anim.SetTrigger("closeLid");
    }
}
