using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using System;

public abstract class Admob : Media
{
    public string AdUnitId;

    public Admob(string AdUnitId)
    {
        this.AdUnitId = AdUnitId;
    }

    public abstract void Initialize();

    public abstract void Show(Action complete, Action fail);

    public abstract void Destroy();

    public abstract void Request();

    public abstract bool IsLoaded();
}
