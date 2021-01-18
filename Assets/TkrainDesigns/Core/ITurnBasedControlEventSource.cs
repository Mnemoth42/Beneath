using UnityEngine.Events;

namespace TkrainDesigns.Core
{
    public interface ITurnBasedControlEventSource
    {
        void OnBeginTurnEventAddListener(UnityAction listener);
        void OnBeginTurnEventRemoveListener(UnityAction listener);
        void OnEndTurnEventAddListener(UnityAction listener);
        void OnEndTurnEventRemoveListener(UnityAction listener);
    }
}