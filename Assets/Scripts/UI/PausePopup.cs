using LOT.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopup : MonoFrame {

    private bool lose;

    public override void Show(object data, bool animated)
    {
        GameManager.Instance.isPause = true;
        base.Show(data, animated);
        lose = (bool)data;
    }

    public void Continue()
    {
        if (!lose)
        {
            OnCloseButtonClicked();
        }
        else
        {
            TaskRunner.Instance.Run(SceneManager.Instance.LoadSceneAsync("TimeModeScene", GeneralOptions.Create("difficultLevel", GameManager.Instance.difficultLevel, "gameMode", GameMode.Time, "isSavedGame", false)));
        }
    }

    public void ResetGame()
    {
        TaskRunner.Instance.Run(SceneManager.Instance.LoadSceneAsync("TimeModeScene", GeneralOptions.Create("difficultLevel", GameManager.Instance.difficultLevel, "gameMode", GameMode.Time, "isSavedGame", false)));
        OnCloseButtonClicked();
    }

    public void OnRate()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public void OnShare()
    {
        NativeShare.Share("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public override void OnCloseButtonClicked()
    {
        base.OnCloseButtonClicked();
        GameManager.Instance.isPause = false;
        Media.Destroy(Media.Type.Banner);
    }
}
