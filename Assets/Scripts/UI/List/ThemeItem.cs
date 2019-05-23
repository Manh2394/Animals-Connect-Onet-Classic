using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeItem : Item
{

    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Image avatar;
    [SerializeField] private Text title;

    private Theme theme;

    public override void UpdateUI(object data)
    {
        theme = (Theme)data;
        lockIcon.SetActive(!theme.unlock);
        avatar.sprite = theme.avatar;
        title.text = theme.name;
    }

    public void OnSelected()
    {
        ThemeResource.Instance.CurrentTheme = theme;
    }

    public void Unlock()
    {
        theme.Unlock();
    }

    public void OnClick()
    {
        if (!theme.unlock)
        {
            if (!Media.RewardedLoaded)
            {
                HUD1.Instance.ShowYesNoPopup("Yes","Ads not load", () => {
 
                }, () =>
                {
                   
                });
                return;
            }

            HUD1.Instance.ShowYesNoPopup("Watch now","Watch a short video and get new item skin!", () => {
                Media.Show(Media.Type.Rewarded, () =>
                {
                    Unlock();
                    OnSelected();
                    GameManager.Instance.mapUI.ChangeTheme();
                    HUD1.Instance.DismissAllFrames();
                    Analytics.LogEvent("Player watch ads to change theme");
                });
            }, () =>
            {
                Analytics.LogEvent("Player not watch ads to change theme");
            });
        }
        else
        {
            OnSelected();
            GameManager.Instance.mapUI.ChangeTheme();
            HUD1.Instance.DismissAllFrames();
        }
    }
}
