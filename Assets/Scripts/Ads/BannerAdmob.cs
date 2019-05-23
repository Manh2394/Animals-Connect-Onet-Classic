using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class BannerAdmob : Admob
{

    private BannerView bannerView;
    private AdRequest request;

    public BannerAdmob(string AdUnitId) : base(AdUnitId)
    {
        Initialize();
    }

    public override void Initialize()
    {
        bannerView = new BannerView(AdUnitId, AdSize.Banner, AdPosition.Bottom);
        // Called when an ad request has successfully loaded.
        bannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        bannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        bannerView.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        Request();
    }

    public override void Show(Action complete, Action fail)
    {
        bannerView.LoadAd(request);
        bannerView.Show();
    }

    public override void Destroy()
    {
        bannerView.Destroy();
    }

    public override void Request()
    {
        //request = new AdRequest.Builder().AddTestDevice("BE9CB91B0643D38D44E8BA04F0116A29").Build();
        request = new AdRequest.Builder().Build();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {

    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

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

    public override bool IsLoaded()
    {
        return true;
    }
}
