using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventView : MonoBehaviour {

    public Screw.EventType eventType;
    public Text text;
    public string zeroConvert = "0";

    private void OnEnable()
    {
        EventDispatcher.Instance.AddListener(eventType, Handle);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(eventType, Handle);
    }

    private void Handle(Screw.EventType eventType, object data)
    {
        text.text = data.ToString();
        if (data.ToString() == "0")
        {
            text.text = zeroConvert;
            text.fontStyle = FontStyle.Bold;
        }
    }
}
