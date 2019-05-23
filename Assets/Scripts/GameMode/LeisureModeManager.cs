using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Phunk.Core;
using System.Linq;
using LOT.Core;

public class LeisureModeManager : GameManager {
    public int balancePoint = 0;
	public override void Initialize (GeneralOptions options, GameScene gameScene) {
        difficultLevel = (DifficultLevel) options["difficultLevel"];
        if (difficultLevel == DifficultLevel.Normal) {
            ShuffeNum = 5;
            SearchNum = 10;
        } else
        {
            ShuffeNum = 10;
            SearchNum = 15;
        }

        base.Initialize (options, gameScene);
	}

    protected override void NewGame ()
    {
        pairNum = (row - 2) * (col - 2)/2;
	    this.matrix = CoreGame.CreateMatrix (row, col);
        currentPairCellIds = CoreGame.GetAvailablePair (this.matrix);
        if (currentPairCellIds.Count == 0)
            ResetMap ();
        gameStrategy = StrategyFactory.CreateInstance (GetGameStrategy (level));
        mapUI.Initialize (matrix, this, GetGameStrategy (level));
    }

    public override void DoPair (Cell cell1, Cell cell2)
    {
        base.DoPair (cell1, cell2);
        score += increaseScore;
        balancePoint += increaseScore;
        if (balancePoint >= Consts.neededScoreNumToExchange)
        {
            balancePoint -= Consts.neededScoreNumToExchange;
            ShuffeNum++;
        }
        float fillAmount = (float) balancePoint/Consts.neededScoreNumToExchange;
        mapUI.UpdateCountDownBar (fillAmount);
    }

    public override void CheckGameState ()
    {
        if (pairNum <= 0)
        {
            StateMachineChange (GameState.EndLevel);
        } else if (pairNum > 0) //else if (pairNum > 0 && currentPairNum == 0)
        {

            currentPairCellIds = CoreGame.GetAvailablePair (matrix);
            if (currentPairCellIds.Count == 0)
            {
                if (ShuffeNum == 0)
                {
                    StateMachineChange (GameState.EndGame);
                    return;
                }
                ShuffeNum--;
                StartCoroutine (ResetMapWithNotice ());
            }
            UpdateGuide ();
        }        
    }

}
