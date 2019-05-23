using UnityEngine;
using LOT.Core;

public class ChallengeModeManager : GameManager {
	public override void Initialize (GeneralOptions options, GameScene gameScene) {
        difficultLevel = (DifficultLevel) options["difficultLevel"];
        if (difficultLevel == DifficultLevel.Normal) {
            ShuffeNum = 6;
            SearchNum = 3;
            matchTime = 240f;
            // matchTime = 40f;
        } else
        {
            ShuffeNum = 10;
            SearchNum = 5;
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

}
