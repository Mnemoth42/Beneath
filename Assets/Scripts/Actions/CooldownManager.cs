using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    Dictionary<string, int> timers = new Dictionary<string, int>();
    public event System.Action onCooldownChanged;

    public int TurnsRemaining(string key)
    {
        if (!timers.ContainsKey(key)) return 0;
        return timers[key];
    }

    public void SetTimer(string key, int duration)
    {
        timers[key] = duration;
        onCooldownChanged?.Invoke();
    }

    public void ResetCooldowns()
    {
        timers.Clear();
    }

    public void AdvanceTimers()
    {
        var newMap = new Dictionary<string, int>();
        var keys = timers.Keys;
        foreach (var timer in keys)
        {
            if (timers[timer]>0)
            {
                newMap[timer] = timers[timer] - 1;
            }
        }

        timers = newMap;
        onCooldownChanged?.Invoke();
    }

}
