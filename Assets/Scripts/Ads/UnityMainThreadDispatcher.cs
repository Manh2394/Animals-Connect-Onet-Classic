using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnityMainThreadDispatcher : MonoBehaviour {

    public static UnityMainThreadDispatcher Instance;

    public static IEnumerator coroutine;

    private void Awake()
    {
        ThemeResource.Instance.Initialize();

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (coroutine != null)
        {
            StartCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void StartCoroutine(Coroutine coroutine)
    {
        StartCoroutine(coroutine);
    }
}
