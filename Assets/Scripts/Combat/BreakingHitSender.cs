﻿using System;
using System.Collections;
using System.Collections.Generic;
using TkrainDesigns.Tiles.Combat;
using TkrainDesigns.Tiles.Control;
using TkrainDesigns.Tiles.Movement;
using UnityEditorInternal;
using UnityEngine;

public class BreakingHitSender : MonoBehaviour
{
    Animator anim;
    BaseController controller;
    
    BaseController target;

    void Awake()
    {
        controller = GetComponent<BaseController>();
        controller.onTargetChanged += SetReceiver;
    }

    void SetReceiver(BaseController obj)
    {
        target = obj;
    }

    void BeginBreakingHit()
    {
        if (target)
        {
            target.gameObject.SetActive(false);
            target.transform.LookAt(transform.position);
            target.gameObject.SetActive(true);
            target.GetComponent<Animator>().SetTrigger("BreakingHit");
        }
    }


 
}