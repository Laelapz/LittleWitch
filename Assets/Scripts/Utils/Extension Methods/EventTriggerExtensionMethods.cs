using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class EventTriggerExtensionMethods
{
    public static void AddListener(this EventTrigger eventTrigger, EventTriggerType eventID, UnityAction<BaseEventData> action)
    {
        for (var i = 0; i < eventTrigger.triggers.Count; i++)
        {
            if (eventTrigger.triggers[i].eventID == eventID)
            {
                eventTrigger.triggers[i].callback.AddListener(action);
                return;
            }
        }

        CreateEventTriggerEntry(eventID, action);
    }

    public static EventTrigger.Entry CreateEventTriggerEntry(EventTriggerType eventID, UnityAction<BaseEventData> action = null)
    {
        EventTrigger.Entry eventTriggerEntry = new EventTrigger.Entry
        {
            eventID = eventID
        };

        if (action == null) return eventTriggerEntry;

        eventTriggerEntry.callback.AddListener(action);

        return eventTriggerEntry;
    }
}
