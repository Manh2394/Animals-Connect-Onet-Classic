using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using LOT.Core;

namespace LOT.Events
{
    public class EventManager : MonoBehaviour {

    private Dictionary <string, List<UnityAction<BaseEvent>>> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init (); 
                }
            }

            return eventManager;
        }
    }

    void Init ()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, List<UnityAction<BaseEvent>>>();
        }
    }

    public static void StartListening (string eventName, UnityAction<BaseEvent> listener)
    {
        List<UnityAction<BaseEvent>> thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.Add (listener);
        } 
        else
        {
            thisEvent = new List<UnityAction<BaseEvent>> ();
            thisEvent.Add (listener);
            instance.eventDictionary.Add (eventName, thisEvent);
        }
    }

    public static void StopListening (string eventName, UnityAction<BaseEvent> listener)
    {
        if (eventManager == null) return;
        List<UnityAction<BaseEvent>> thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            foreach (var item in thisEvent)
            {
                if (item == listener)
                {
                    thisEvent.Remove (listener);
                    return;
                }
            }
        }
    }

    public static void TriggerEvent (string eventName, BaseEvent param)
    {
        List<UnityAction<BaseEvent>> thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            foreach (var item in thisEvent)
            {
                item.Invoke (param);
            }
        }
    }

    public static void TriggerEvent (BaseEvent e)
    {
        string eventType = e.type;
        List<UnityAction<BaseEvent>> listeners = null;
        instance.eventDictionary.TryGetValue (eventType,out listeners);

        foreach (var listener in listeners)
        {
            listener.Invoke (e);
        }
    }

    public void TriggerEvent (string type, GeneralOptions generalOptionsParam = null)
    {
        TriggerEvent (BaseEvent.CreateOptions (type, this, generalOptionsParam));
    }

    public void TriggerEvent (string type, int intParam)
    {
        TriggerEvent (BaseEvent.CreateSimple (type, this, intParam));
    }

    public void TriggerEvent (string type, float floatParam)
    {
        TriggerEvent (BaseEvent.CreateSimple (type, this, 0, floatParam));
    }

    public void TriggerEvent (string type, string stringParam)
    {
        TriggerEvent (BaseEvent.CreateSimple (type, this, 0, 0, stringParam));
    }

    public void TriggerEvent(string type, object objectParam)
    {
        TriggerEvent (BaseEvent.CreateSimple (type, this, 0, 0, "", objectParam));
    }

    public void TriggerTimedEvent (string type, GeneralOptions generalOptionsParam = null)
    {
        TriggerEvent (BaseEvent.CreateTimedOptions (type, this, generalOptionsParam));
    }

    public void TriggerTimedEvent (string type, int intParam)
    {
        TriggerEvent (BaseEvent.CreateTimedSimple (type, this, intParam));
    }

    public void TriggerTimedEvent (string type, float floatParam)
    {
        TriggerEvent (BaseEvent.CreateTimedSimple (type, this, 0, floatParam));
    }

    public void TriggerTimedEvent (string type, string stringParam)
    {
        TriggerEvent (BaseEvent.CreateTimedSimple (type, this, 0, 0, stringParam));
    }

    public void TriggerTimedEvent(string type, object objectParam)
    {
        TriggerEvent (BaseEvent.CreateTimedSimple (type, this, 0, 0, "", objectParam));
    }

    public void ForwardTriggerEvent (BaseEvent e)
    {
        TriggerEvent (BaseEvent.CloneWithNewSender (e, this));
    }

}
}