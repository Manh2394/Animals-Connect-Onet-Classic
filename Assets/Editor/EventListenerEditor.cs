using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(EventListener))]
public class EventListenerEditor : Editor 
{
    EventListener listener;

    private void OnEnable()
    {
        listener = target as EventListener;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        string[] options = Enum.GetNames(typeof(Screw.EventType));
        int selected = Array.IndexOf(options, listener.eventName);
        int selection = EditorGUILayout.Popup("Event", selected, options);
        if (selection != selected && selection >= 0)
        {
            listener.eventName = options[selection];
            EditorUtility.SetDirty(listener);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("handler"));

        serializedObject.ApplyModifiedProperties();
    }
}
