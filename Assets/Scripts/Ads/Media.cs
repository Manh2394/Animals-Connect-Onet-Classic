using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using System;

public class Media {

    private static readonly string AndroidAppId = "ca-app-pub-4951039773361207~4120321993";
    private static readonly string IOSAppId;

    private static readonly string BannerAndroidAdUnitId = "ca-app-pub-3940256099942544/6300978111";
    private static readonly string BannerIOSAdUnitId;

    private static readonly string InterstitialAndroidAdUnitId = "ca-app-pub-3940256099942544/1033173712";
    private static readonly string InterstitialIOSAdUnitId;

    private static readonly string RewardedAndroidAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    private static readonly string RewardedIOSAdUnitId;

    private static string AppId
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return AndroidAppId;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return IOSAppId;
            }
            return "";
        }
    }

    private static string BannerAdUnitId
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return BannerAndroidAdUnitId;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return BannerIOSAdUnitId;
            }
            return "";
        }
    }

    private static string InterstitialAdUnitId
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return InterstitialAndroidAdUnitId;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return InterstitialIOSAdUnitId;
            }
            return "";
        }
    }

    private static string RewardedAdUnitId
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return RewardedAndroidAdUnitId;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return RewardedIOSAdUnitId;
            }
            return "";
        }
    }

    public enum Type
    {
        Banner,
        Interstitial,
        Rewarded
    }

    private static Admob bannerAdmob;
    private static Admob interstitialAdmob;
    private static Admob rewardedAdmob;

    private static List<Admob> admobs = new List<Admob>();

    public static bool BannerLoaded
    {
        get
        {
            return bannerAdmob.IsLoaded();
        }
    }

    public static bool InterstitialLoaded
    {
        get
        {
            return interstitialAdmob.IsLoaded();
        }
    }

    public static bool RewardedLoaded
    {
        get
        {
            return rewardedAdmob.IsLoaded();
        }
    }

    // Use this for initialization
    public static void Initialize() {
        MobileAds.Initialize(AppId);

        bannerAdmob = new BannerAdmob(BannerAdUnitId);
        interstitialAdmob = new InterstitialAdmob(InterstitialAdUnitId);
        rewardedAdmob = new RewardedAdmob(RewardedAdUnitId);

        admobs = new List<Admob>()
        {
            bannerAdmob,
            interstitialAdmob,
            rewardedAdmob,
        };
    }

    public static void Show(Type type, Action onComplete = null, Action onFail = null)
    {
        switch (type)
        {
            case Type.Banner:
                bannerAdmob.Show(onComplete, onFail);
                break;

            case Type.Interstitial:
                interstitialAdmob.Show(onComplete, onFail);
                break;

            case Type.Rewarded:
                rewardedAdmob.Show(onComplete, onFail);
                break;
        }
    }

    public static void RequestAll()
    {
        foreach (var item in admobs)
        {
            if (!item.IsLoaded())
            {
                item.Request();
            }
        }
    }

    public static void Destroy(Type type)
    {
        switch (type)
        {
            case Type.Banner:
                if (bannerAdmob != null)
                    bannerAdmob.Destroy();
                break;

            case Type.Interstitial:
                if (interstitialAdmob != null)
                    interstitialAdmob.Destroy();
                break;

            case Type.Rewarded:
                if (rewardedAdmob != null)
                    rewardedAdmob.Destroy();
                break;
        }
    }
}
