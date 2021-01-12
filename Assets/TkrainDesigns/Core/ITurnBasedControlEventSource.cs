using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ITurnBasedControlEventSource
{
    void OnBeginTurnEventAddListener(UnityAction listener);
    void OnBeginTurnEventRemoveListener(UnityAction listener);
    void OnEndTurnEventAddListener(UnityAction listener);
    void OnEndTurnEventRemoveListener(UnityAction listener);
}
