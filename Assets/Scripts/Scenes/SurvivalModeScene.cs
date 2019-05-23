using UnityEngine;
using System.Collections;
using LOT.Core;
using System;

public class SurvivalModeScene : GameScene {
    private bool isOpenBuyDialog = false;
    public GameObject buyHintDialog;
    public GameObject buyTimeDialog;
    public GameObject buyShuffleDialog;
    public void OnClickBuyDialog (GameObject dialog)
    {
        if (dialog.GetComponent <Animation>().isPlaying)
            return;
        isOpenBuyDialog = !isOpenBuyDialog;
        string animationName = isOpenBuyDialog ? "OpenBuyDialog" : "CloseBuyDialog";
        dialog.GetComponent <Animation>().Play (animationName);
    }

    public void OnClickBuyHint (int num)
    {
        if (num == 1 && gameManager.score >= 1500) {
            gameManager.score -= 1500;
            gameManager.mapUI.UpdateScoreTxt ();
            gameManager.SearchNum += num;
            //gameManager.mapUI.UpdateSearchNumTxt (gameManager.searchNum.ToString ());
            OnDialogClose (buyHintDialog);
        } else if (num == 5 && gameManager.score >= 5000) {
            gameManager.score -= 1500;
            gameManager.mapUI.UpdateScoreTxt ();
            gameManager.SearchNum += num;
            //gameManager.mapUI.UpdateSearchNumTxt (gameManager.searchNum.ToString ());
            OnDialogClose (buyHintDialog);
        }
    }

    public void OnClickBuyTime (int num)
    {
        if (num == 30 && gameManager.score >= 3500) {
            gameManager.remainTime += num;
            gameManager.score -= 3500;
            gameManager.mapUI.UpdateScoreTxt ();
            gameManager.mapUI.UpdateCountDownBar (gameManager.remainTime/gameManager.matchTime);
            OnDialogClose (buyTimeDialog);
        } else if (num == 5 && gameManager.score >= 5000) {
            gameManager.remainTime += num;
            gameManager.score -= 3500;
            gameManager.mapUI.UpdateScoreTxt ();
            gameManager.mapUI.UpdateCountDownBar (gameManager.remainTime/gameManager.matchTime);
            OnDialogClose (buyTimeDialog);
        }
    }

    public void OnClickBuyShuffle (int num)
    {
        if (num == 1 && gameManager.score >= 2500) {
            gameManager.ShuffeNum += num;
            gameManager.score -= 2500;
            //gameManager.mapUI.UpdateShuffeNumTxt (gameManager.shuffeNum.ToString ());
            gameManager.mapUI.UpdateScoreTxt ();
            OnDialogClose (buyShuffleDialog);
        } else if (num == 3 && gameManager.score >= 6000) {
            gameManager.ShuffeNum += num;
            gameManager.score -= 6000;
            //gameManager.mapUI.UpdateShuffeNumTxt (gameManager.shuffeNum.ToString ());
            gameManager.mapUI.UpdateScoreTxt ();
            OnDialogClose (buyShuffleDialog);
        }
    }

}
