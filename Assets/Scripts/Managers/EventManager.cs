using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private bool[] events;

    private void Start()
    {
        events = new bool[Enum.GetNames(typeof(DialogueEvents)).Length];
        for (int i = 0; i < events.Length; i++)
        {
            events[i] = false;
        }
    }
    public void EventOccured(DialogueEvents e)
    {
        if ((int)e != -1)
        {
            events[(int)e] = true;
        }
    }

    public bool RequiredEventsOccured(params DialogueEvents[] eve)
    {
        for (int i = 0; i < eve.Length; i++)
        {
            if ((int)eve[i] == -1)
            {
                return false;
            }
            if (!events[(int)eve[i]])
            {
                return false;
            }
        }
        return true;
    }
}

public enum DialogueEvents
{
    NoEvent = -1,
    Event1 = 0,
    Event2 = 1,
    Event3 = 2,
    Event4 = 3,
};
