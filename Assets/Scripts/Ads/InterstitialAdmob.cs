using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class InterstitialAdmob : Admob
{
    private InterstitialAd interstitial;
    private AdRequest request;

    private Action OnComplete;
    private Action OnFail;

    public InterstitialAdmob(string AdUnitId) : base(AdUnitId)
    {
        Initialize();
    }

    public override void Initialize()
    {
        interstitial = new InterstitialAd(AdUnitId);
        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        Request();
    }

    public override void Destroy()
    {
        interstitial.Destroy();
    }

    public override void Request()
    {
        //AdRequest request = new AdRequest.Builder().AddTestDevice("BE9CB91B0643D38D44E8BA04F0116A29").Build();
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    public override void Show(Action complete, Action fail)
    {
        this.OnComplete = complete;
        this.OnFail = fail;

        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            if (OnComplete != null)
            {
                OnComplete.Invoke();
            }
        }
        else
        {
            if (fail != null)
            {
                fail.Invoke();
            }
        }
    }

    public override bool IsLoaded()
    {
        return interstitial.IsLoaded();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Request();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
}
