using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowProcess : MonoBehaviour {

    
    public Screw.EventType eventDispatcher;
    private Image process;

    // Use this for initialization
    void Start()
    {
        EventDispatcher.Instance.AddListener(eventDispatcher, Handle);

		process = GetComponent<Image>();
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveListener(eventDispatcher, Handle);
    }

    private void Handle(Screw.EventType e, object data)
    {
		if (process != null)
        {
			process.fillAmount = (float)data;
        }
    }
}
