using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    private BannerView bannerView;

    public Text text;
    public Image r;
    public Image i;

    private void Awake()
    {
        Media.Initialize();
    }

    public void Banner()
    {
        Media.Show(Media.Type.Banner, null, null);
    }

    public void Rewarded()
    {
        Media.Show(Media.Type.Rewarded, () =>
        {
            text.text = "Rewarded complete";
            Debug.Log("Rewarded complete ------------------------------------------------------");
        }, () =>
        {
            text.text = "Rewarded fail";
            Debug.Log("Rewarded fail ---------------------------------------------------------");
        });
    }

    public void Interstitial()
    {
        Media.Show(Media.Type.Interstitial, null, null);
    }

    private void Update()
    {
        r.color = Media.RewardedLoaded ? Color.red : Color.white;
        i.color = Media.InterstitialLoaded ? Color.red : Color.white;
    }

}
