﻿using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Phunk.Core;
using System.Linq;
using LOT.Core;

public class TimeModeManager : GameManager {
	public override void Initialize (GeneralOptions options, GameScene gameScene) {
        difficultLevel = (DifficultLevel) options["difficultLevel"];
        if (difficultLevel == DifficultLevel.Normal) {
            SearchNum = 5;
            TimeNum = 3;
            ShuffeNum = 10;
            matchTime = 135;
            // matchTime = 20f;
        } else
        {
            SearchNum = 5;
            TimeNum = 3;
            ShuffeNum = 10;
            matchTime = 360;
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
        if (difficultLevel == DifficultLevel.Hard && level > 9) {
            matchTime -= 10;
            if (matchTime < 180) {
                matchTime = 180;
            }
        }
        remainTime = matchTime;
    }
	// Update is called once per frame
    void Update () {
        if (isPause)
            return;
	    remainTime -= Time.deltaTime;
        if (remainTime <= 0)
        {
            if ((GameState)fsm.state != GameState.EndGame)
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
                StartCoroutine (ResetMapWithNotice ());
            }
            UpdateGuide ();
        }        
    }

}
