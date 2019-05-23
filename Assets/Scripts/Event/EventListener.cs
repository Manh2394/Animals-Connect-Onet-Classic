using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EventListener : MonoBehaviour
{
    public string eventName;
    public UnityEvent handler;

    Screw.EventType eventType;

    void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(Screw.EventType));
        int index = Array.IndexOf(names, eventName);
        if (index < 0)
        {
            eventType = Screw.EventType.NONE;
        }
        else
        {
            eventType = (Screw.EventType)Enum.GetValues(typeof(Screw.EventType)).GetValue(index);
        }

        if (eventType != Screw.EventType.NONE)
        {
            EventDispatcher.Instance.AddListener(eventType, OnEvent);
        }
    }

    void OnDisable()
    {
        if (eventType != Screw.EventType.NONE)
        {
            EventDispatcher.Instance.RemoveListener(eventType, OnEvent);
        }
    }

    void OnEvent(Screw.EventType key, object data)
    {
        handler.Invoke();
    }
}
