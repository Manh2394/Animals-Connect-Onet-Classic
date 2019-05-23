using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Phunk.Core;
using System.Linq;
using LOT.Core;
using System.Collections;

public enum GameStrategy
{
    Normal = 1,
    Bottom,
    Top,
    Left,
    Right,
    LeftAndRight,
    TopAndBottom,
    XCenter,
    YCenter,
}

public enum GameState
{
    Entry,
    LoadSavedGame,
    StartGame,
    EnterLevel,
    EndLevel,
    Matching,
    ResetMap,
    CreateNextLevel,
    EndGame
}
public enum GameMode
{
    Leisure,
    Time,
    Challenge,
    Survival
}

public enum DifficultLevel
{
    Normal,
    Hard
}

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    protected int[,] matrix;
    public MapUI mapUI;
	protected int notBarrier = 0;
    protected int pairNum;
    public float remainTime;
    public Dictionary<int, int> currentPairCellIds = new Dictionary<int, int> ();
    public int level = 1;
    public int score = 0;
    protected IGameStrategy gameStrategy;

    private int shuffeNum;
    private int searchNum;
    private int timeNum;

    public CustomFSMManager fsm;
    public StateMachineChangeDelegate StateMachineChange;
    protected int row = 10;
    protected int col = 16;
    public GameMode gameMode;
    public DifficultLevel difficultLevel;
    public bool isPause = false;
    public bool isSavedGame = false;
    public int increaseScore = 100;
    public GameScene gameScene;
    public float matchTime;

    public int ShuffeNum
    {
        get
        {
            return shuffeNum;
        }

        set
        {
            shuffeNum = value;
            EventDispatcher.Instance.Dispatch(Screw.EventType.SHUFFE_CHANGE, value);
        }
    }
    public int SearchNum
    {
        get
        {
            return searchNum;
        }

        set
        {
            searchNum = value;
            EventDispatcher.Instance.Dispatch(Screw.EventType.SEARCH_CHANGE, value);
        }
    }

    public int TimeNum
    {
        get
        {
            return timeNum;
        }

        set
        {
            timeNum = value;
            EventDispatcher.Instance.Dispatch(Screw.EventType.TIME_CHANGE, value);
        }
    }

    public virtual void Awake () {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
    }
	// Use this for initialization
	public virtual void Initialize (GeneralOptions options, GameScene gameScene) {
        this.gameScene = gameScene;
        difficultLevel = (DifficultLevel) options["difficultLevel"];
        gameMode = (GameMode) options["gameMode"];
        isSavedGame = (bool) options ["isSavedGame"];
        SetupRowColOfBoard (difficultLevel);
        // setup custom fsm
        fsm = gameObject.AddComponent<CustomFSMManager> ();
        fsm.Initialize (typeof(GameState), this, false);
        StateMachineChange = new StateMachineChangeDelegate (fsm.StateMachineChange);
        if (isSavedGame)
        {
            StateMachineChange (GameState.LoadSavedGame);
        } else
        {
            StateMachineChange (GameState.StartGame);
        }
	}

    private void SetupRowColOfBoard (DifficultLevel difficultLevel)
    {
        switch (difficultLevel)
        {
            case DifficultLevel.Normal:
                row = 7;
                col = 12;
                // row = 6;
                // col = 8;
                break;
            case DifficultLevel.Hard:
                row = 9;
                col = 16;
                break;
        }
        
    }
	
    protected virtual void NewGame ()
    {
        pairNum = (row - 2) * (col - 2)/2;
	    this.matrix = CoreGame.CreateMatrix (row, col);
        if (CoreGame.GetAvailablePair (this.matrix).Count == 0)
            ResetMap ();
        gameStrategy = StrategyFactory.CreateInstance (GetGameStrategy (level));
        mapUI.Initialize (matrix, this, (GameStrategy)level);
        remainTime = matchTime;
    }

    protected GameStrategy GetGameStrategy (int level)
    {
        if (level > 9)
        {
            return GameStrategy.YCenter;            
        }
        return (GameStrategy) level;
        // return GameStrategy.Bottom;
        // return GameStrategy.YCenter;
    }
    public virtual void DoPair (Cell cell1, Cell cell2)
    {
        pairNum--;
        gameStrategy.DoPair (matrix, cell1, cell2);
    }

    public void UpdateGuide ()
    {
        KeyValuePair<int, int> firstPair = currentPairCellIds.FirstOrDefault ();
        mapUI.UpdateGuideText (BaseUtil.GetCellById (firstPair.Key, row, col) + "-" + BaseUtil.GetCellById (firstPair.Value, row, col));        
    }

    public virtual void CheckGameState ()
    {
    }

    public IEnumerator ResetMapWithNotice ()
    {
        StateMachineChange (GameState.ResetMap);
        yield return new WaitForSeconds (2f);
        ResetMap ();
        StateMachineChange (GameState.EnterLevel);
    }

    public void ResetMap()
    {
        for (int i = 1; i < row - 1; i++)
            for (int j = 1; j < col - 1; j++)
                if (matrix[i,j] > notBarrier)
                {
                    bool is_swaped = false;
                    for (int i2 = 1; i2 < row - 1; i2++)
                    {
                        for (int j2 = 1; j2 < col - 1; j2++)
                        {
                            if (i2 <= i && j2 <= j) continue;
                            if (matrix[i2, j2] > notBarrier)
                            {
                                if (UnityEngine.Random.Range(0, 2) == 0)
                                {
                                    Swap(new Cell (i, j), new Cell (i2, j2));
                                }
                                is_swaped = true;
                                break;
                            }
                        }
                        if (is_swaped) break;
                    }
                }
        currentPairCellIds = CoreGame.GetAvailablePair (matrix);
        if (currentPairCellIds.Count == 0)
            ResetMap ();

        Debug.Log("Reset map");
    }
    public void Swap (Cell cell1, Cell cell2)
    {
        int cardTypeOfCell1 = matrix[cell1.row, cell1.col];
        int cardTypeOfCell2 = matrix[cell2.row, cell2.col];
        matrix[cell1.row, cell1.col] = cardTypeOfCell2;
        matrix[cell2.row, cell2.col] = cardTypeOfCell1;
        Log.Verbose ("swap: " + cell1 + "-" + cell2);
        Log.Verbose ("value after: " + matrix[cell1.row, cell1.col] + "-" + matrix[cell2.row, cell2.col]);
        mapUI.SwapCard (cell1, cell2);
        
    }

	public List<Cell> CheckPairTwoCell (Cell cell1, Cell cell2)
    {
		return CoreGame.GetWayBetweenTwoCell (matrix, cell1, cell2);
	}
    public void decreaseSearchNum ()
    {
        if (SearchNum > 0)
            SearchNum--;
    }

    protected virtual void StateMachineEnter_StartGame (Enum state, GeneralOptions options = null)
    {
        level = 0;
        NewGame ();
        StateMachineChange (GameState.CreateNextLevel);
    }

    void StateMachineEnter_LoadSavedGame (Enum state, GeneralOptions options = null)
    {
        Debug.Log("StateMachineEnter_LoadSavedGame");

        level = Settings.Level;
        gameStrategy = StrategyFactory.CreateInstance (GetGameStrategy (level));
        score = Settings.Score;
        remainTime = Settings.RemainTime;
        SearchNum = Settings.SearchNum;
        ShuffeNum = Settings.ShuffeNum;
        pairNum = Settings.PairNum;
        TimeNum = Settings.TimeNum;

        int[] tempMatrix =Settings.Matrix;
        Debug.LogWarning ("length: " + tempMatrix.Length);
        matrix = new int[row, col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Debug.LogWarning ("i: " + i + " j: " + j + " value: " + matrix[i, j]);
                matrix[i, j] = tempMatrix[col * i + j];
            }
        }
        mapUI.Initialize (matrix, this, (GameStrategy)level);
        currentPairCellIds = CoreGame.GetAvailablePair (matrix);
        StateMachineChange (GameState.EnterLevel);
    }

    void StateMachineEnter_EndLevel (Enum state, GeneralOptions options = null)
    {
        SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.levelComplete);
        StateMachineChange (GameState.CreateNextLevel);
        Media.Show(Media.Type.Interstitial);
    }


    void StateMachineEnter_CreateNextLevel (Enum state, GeneralOptions options = null)
    {
        level++;
        SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.levelComplete);
        NewGame ();
        StateMachineChange (GameState.EnterLevel);
    }

    void StateMachineEnter_EndGame(Enum state, GeneralOptions options = null)
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.lose);
        Debug.LogWarning(gameMode.ToString() + difficultLevel.ToString());
        int bestScore = PlayerPrefs.GetInt(gameMode.ToString() + difficultLevel.ToString());
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(gameMode.ToString() + difficultLevel.ToString(), bestScore);
        }
        mapUI.ShowLoseDialog();
    }

    void StateMachineExit_StartGame (Enum state, GeneralOptions options = null)
    {
        SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.levelComplete);
    }

    public void SaveGame ()
    {
        Settings.GameMode = (int)gameMode;
        Settings.DifficultLevel = (int)difficultLevel;
        Settings.Level = (int)level;
        Settings.Score = (int)score;
        Settings.RemainTime = (int)remainTime;
        Settings.SearchNum = (int)SearchNum;
        Settings.ShuffeNum = (int)ShuffeNum;
        Settings.PairNum = (int)pairNum;
        Settings.TimeNum = TimeNum;


        row = this.matrix.GetLength (0);
        col = this.matrix.GetLength (1);
        int[] tempMatrix = new int[row * col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                tempMatrix[col * i + j] = matrix[i, j];
            }
        }
        Settings.Matrix = tempMatrix;
    }

    public void forlife(GameObject obj)
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.levelComplete);
        NewGame();
        StateMachineChange(GameState.EnterLevel);
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Media.RequestAll();
        }

        SaveGame();
    }

    void OnDestroy()
    {
        SaveGame();
    }

}
