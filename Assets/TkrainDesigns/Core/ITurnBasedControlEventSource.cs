using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TkrainDesigns.Core.Interfaces
{
    public interface ITurnBasedControlEventSource
    {
        void OnBeginTurnEventAddListener(UnityAction listener);
        void OnBeginTurnEventRemoveListener(UnityAction listener);
        void OnEndTurnEventAddListener(UnityAction listener);
        void OnEndTurnEventRemoveListener(UnityAction listener);
    }
}