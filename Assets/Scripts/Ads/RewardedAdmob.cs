using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewardedAdmob : Admob
{
    private RewardBasedVideoAd rewardBasedVideo;
    private AdRequest request;

    private Action OnComplete;
    private Action OnFail;

    public RewardedAdmob(string AdUnitId) : base(AdUnitId)
    {
        Initialize();
    }

    public override void Initialize()
    {
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
        Request();
    }

    public override void Destroy()
    {

    }

    public override void Request()
    {
        //request = new AdRequest.Builder().AddTestDevice("BE9CB91B0643D38D44E8BA04F0116A29").Build();
        request = new AdRequest.Builder().Build();
        this.rewardBasedVideo.LoadAd(request, AdUnitId);
    }

    public override void Show(Action complete, Action fail)
    {
#if UNITY_EDITOR
        if (OnComplete != null)
        {
            OnComplete.Invoke();
        }
#endif

        this.OnComplete = complete;
        this.OnFail = fail;

        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
        else
        {
            if (OnFail != null)
            {
                OnFail.Invoke();
            }
        }
    }

    public override bool IsLoaded()
    {
        return rewardBasedVideo.IsLoaded();
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {

    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
       
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {

    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {

    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        this.Request();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        UnityMainThreadDispatcher.coroutine = Complete();
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {

    }

    IEnumerator Complete()
    {
        if (OnComplete != null)
        {
            OnComplete.Invoke();
        }
        yield return null;
    }
}
