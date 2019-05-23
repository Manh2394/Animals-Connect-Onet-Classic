using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HUD1 : HUD {

    private static HUD1 _instance;
    public static HUD1 Instance{
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType<HUD1>();
                return _instance;
            }else
                return _instance;
        }
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        if (_instance == null)
        {
            _instance = this;
        }
    }

    [AutoAssign]
    public PausePopup pausePopup;

    [AutoAssign]
    public YesNoPopup yesNoPopup;

    [AutoAssign]
    public ChangeThemePopup changeThemePopup;

    [AutoAssign]
    public LosePopup losePopup;

    public void ShowPausePopup(bool lose = false)
    {
        this.Show(pausePopup, true, lose, false);
    }

    public void ShowYesNoPopup(string yesText, string title, Action actionYes, Action actionNo)
    {
        this.Show(yesNoPopup, true, null, false);
        yesNoPopup.SetContent(yesText, title, actionYes, actionNo);
    }

    public void ShowChangeTheme()
    {
        this.Show(changeThemePopup, true, null, false);
    }

    public void ShowLosePopup()
    {
        this.Show(losePopup, true, null, false);
    }
}
