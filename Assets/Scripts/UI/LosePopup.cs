using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LOT.Core;

public class LosePopup : MonoFrame
{

    public void GoHome()
    {
        TaskRunner.Instance.Run(SceneManager.Instance.LoadSceneAsync("Home", null));
    }

    public void ResetGame()
    {
        TaskRunner.Instance.Run(SceneManager.Instance.LoadSceneAsync("TimeModeScene", GeneralOptions.Create("difficultLevel", GameManager.Instance.difficultLevel, "gameMode", GameMode.Time, "isSavedGame", false)));
        OnCloseButtonClicked();
    }
}
