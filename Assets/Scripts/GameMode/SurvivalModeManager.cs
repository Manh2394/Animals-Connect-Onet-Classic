using UnityEngine;
using System;
using LOT.Core;

public class  SurvivalModeManager : GameManager {
	public override void Initialize (GeneralOptions options, GameScene gameScene) {
        difficultLevel = (DifficultLevel) options["difficultLevel"];
        if (difficultLevel == DifficultLevel.Normal) {
            ShuffeNum = 3;
            SearchNum = 3;
            matchTime = 480f;
        } else
        {
            ShuffeNum = 6;
            SearchNum = 6;
            matchTime = 720f;
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
        // gameMode = GameMode.Top;
        mapUI.Initialize (matrix, this, GetGameStrategy (level));
    }

    void Update () {
        if (isPause)
            return;
	    remainTime -= Time.deltaTime;
        if (remainTime <= 0)
        {
            StateMachineChange (GameState.EndGame);
        } else
        {
            mapUI.UpdateCountDownBar (remainTime/this.matchTime);
        }
	}
    public override void DoPair (Cell cell1, Cell cell2)
    {
        base.DoPair (cell1, cell2);
        score += increaseScore;
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

    protected override void StateMachineEnter_StartGame (Enum state, GeneralOptions options = null)
    {
        remainTime = this.matchTime;
        level = 0;
        NewGame ();
        StateMachineChange (GameState.CreateNextLevel);
    }

}
