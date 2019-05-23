using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LOT.Core;
using Assets.Phunk.Core;
using System.Linq;
using DG.Tweening;
//using UnityEngine.SocialPlatforms;
//using GooglePlayGames;
//using GoogleMobileAds.Api;
using UnityEngine.Analytics;
//using GoogleMobileAds.Common;

public class MapUI : MonoBehaviour {
    public string interstitialAdsId = "ca-app-pub-6896848237501489/2631822453";
    public GameObject prefabPikachu;
    public GameObject firstTarget;
    public GameObject secondTarget;
    private Vector3[,] cellsPos;
    private int rowNum;
    private int colNum;
    private float minXPos;
    private float minYPos;
    private float cellHeight;
    private float cellWidth;
    private int[,] matrix;
    private Cell firstSelectCell = null;
    private Cell secondSelectCell;
    public List<Cell> cellsOnLine = new List<Cell> ();
    private GameManager gameManager;
    private GameObject selectedGameObject;
    public LineRenderer line;
    public Image countDownBar;
    public Text guideText;
    public Text LevelText;
    public Text ScoreText;
    public Text newScoreTxt;
    public Text bestScoreTxt;
    public Text money;

    public Text countSearchTxt;
    public Text countShuffeTxt;
    public Text counttimeTxt;
    public int stagMoney;
    public Image loadVideo;
    public Color colorPress;

    public GameObject UIpause;
    public GameObject UIshop;

    public GameObject loseDialog;
    public Dictionary<int, GameObject> cells = new Dictionary<int, GameObject> ();

    private GameStrategy gameStrategy;
    private int row;
    private int col;
    private bool hasHint = false;
    private Text countdownText;
    //InterstitialAd interstitialAds;
    //RewardBasedVideoAd rewardBasedVideo;
    bool rewardBasedEventHandlersSet;
    public Text log;

    private int check;
    public void Initialize (int[,] matrix, GameManager gameManager, GameStrategy gameStrategy)
    {
		

        this.gameManager = gameManager;
        this.matrix = matrix;
        row = this.matrix.GetLength (0);
        col = this.matrix.GetLength (1);
        this.gameStrategy = gameStrategy;
        LevelText.text = "Lv." + gameManager.level;
        ScoreText.text = "Score: " + gameManager.score;
        rowNum = matrix.GetLength (0);
        colNum = matrix.GetLength (1);
        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        cellWidth = (cameraWidth * 8/9) / (col);
        cellHeight = (cameraHeight * 8/9) / row;
      
        minXPos = -colNum * (cellWidth - 0.1f) / 2;
        minYPos = -rowNum * (cellHeight - 0.1f) / 2;
        cellsPos = CalCellPos (rowNum, colNum, minXPos, minYPos, cellWidth, cellHeight);

        Sprite firstTargetSprite = firstTarget.GetComponent<SpriteRenderer> ().sprite;
        Sprite secondTargetSprite = secondTarget.GetComponent<SpriteRenderer> ().sprite;
        firstTarget.transform.localScale = new Vector3(Mathf.Abs(cellWidth * 1.0f / firstTargetSprite.bounds.size.x), Mathf.Abs (- cellHeight * 1.0f / firstTargetSprite.bounds.size.y), 1);
        secondTarget.transform.localScale = new Vector3(Mathf.Abs(cellWidth * 1.0f / secondTargetSprite.bounds.size.x), Mathf.Abs (- cellHeight * 1.0f / secondTargetSprite.bounds.size.y), 1);

        CreateCardBoard ();
        //RequestInterstitial ();


    }

	// Use this for initialization
	void Awake () {
        countdownText = countDownBar.GetComponentInChildren <Text>();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && gameManager.fsm.state == (int) GameState.EnterLevel && !gameManager.isPause) 
        {
            Vector3 convertPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(convertPos.x, convertPos.y);
            int mouse_col = (int)((touchPos.x - minXPos + cellWidth / 2) / (cellWidth));
            int mouse_row = (int)((touchPos.y - minYPos + cellHeight / 2) / (cellHeight));
            Log.Verbose(mouse_col + " " + mouse_row);
            if (mouse_col >= colNum - 1 || mouse_col <= 0 || mouse_row >= rowNum -1 || mouse_row <= 0)
                return;
            if (firstSelectCell == null)
            {
                firstSelectCell = new Cell (mouse_row, mouse_col);                
            } else
            {
                secondSelectCell = new Cell (mouse_row, mouse_col);
                cellsOnLine.Clear ();
                float startTime = Time.realtimeSinceStartup;
                cellsOnLine = gameManager.CheckPairTwoCell (firstSelectCell, secondSelectCell);
                float endTime = Time.realtimeSinceStartup;
                // Log.VerboseError ("benmark: " + (endTime - startTime));
                if (cellsOnLine.Count > 0) {
                    gameManager.StateMachineChange (GameState.Matching);
                    gameManager.DoPair (firstSelectCell, secondSelectCell);
                    StartCoroutine (DoPair ());
                    hasHint = false;
                } else
                {
                    firstSelectCell = secondSelectCell;
                }
            }
            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
            HighLightSelectedCard ();
        }
	
	}

    IEnumerator DoPair ()
    {
        // Debug.LogError ("start dopair UI");
        GameObject cel1;
        cells.TryGetValue (BaseUtil.GetCellId (firstSelectCell, row, col), out cel1);
        GameObject cel2;
        cells.TryGetValue (BaseUtil.GetCellId (secondSelectCell, row, col), out cel2);

        DrawLine ();
        yield return new WaitForSeconds (0.05f);
        ScoreText.text = "Score: " + gameManager.score;
        cel1.transform.DOLocalRotate (new Vector3 (0,0,90), 0.2f, RotateMode.FastBeyond360);
        cel2.transform.DOLocalRotate (new Vector3 (0,0,90), 0.2f, RotateMode.FastBeyond360);
        yield return new WaitForSeconds (0.2f);
        line.gameObject.SetActive (false);
        cel1.SetActive (false);
        cel2.SetActive (false);
        firstTarget.SetActive (false);
        secondTarget.SetActive (false);
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.getScore);
        yield return null;
        switch (gameStrategy)
        {
            case GameStrategy.Bottom:
            case GameStrategy.Top:
            case GameStrategy.TopAndBottom:
            case GameStrategy.YCenter:
                for (int i = 1; i < rowNum; i++)
                {
                    FillCardToCell (matrix[i, firstSelectCell.col], new Cell (i, firstSelectCell.col));
                }
                if (firstSelectCell.col != secondSelectCell.col)
                {
                    for (int i = 1; i < rowNum; i++)
                    {
                        FillCardToCell (matrix[i, secondSelectCell.col], new Cell (i, secondSelectCell.col));
                    }
                }
                break;
            case GameStrategy.Left:
            case GameStrategy.Right:
            case GameStrategy.LeftAndRight:
            case GameStrategy.XCenter:
                for (int i = 1; i < colNum; i++)
                {
                    FillCardToCell (matrix[firstSelectCell.row, i], new Cell (firstSelectCell.row, i));
                }
                if (firstSelectCell.row != secondSelectCell.row)
                {
                    for (int i = 1; i < colNum; i++)
                    {
                        FillCardToCell (matrix[secondSelectCell.row, i], new Cell (secondSelectCell.row, i));
                    }
                }
                break;
        }
        yield return null;
        SetNonSelectCell (firstSelectCell);
        SetNonSelectCell (secondSelectCell);
        firstSelectCell = secondSelectCell = null;
        yield return null;
        gameManager.CheckGameState ();
        if (gameManager.fsm.state == (int) GameState.Matching) {
            gameManager.StateMachineChange (GameState.EnterLevel);
        }
    }

    public void UpdateScoreTxt ()
    {
        ScoreText.text = "Score: " + gameManager.score;
    }

    private void SetNonSelectCell (Cell cell)
    {
        GameObject cellGO;
        cells.TryGetValue (BaseUtil.GetCellId (firstSelectCell, row, col), out cellGO);
        cellGO.transform.GetChild(0).GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);        
    }

    public void DrawLine ()
    {
        line.gameObject.SetActive (true);
        line.SetVertexCount (cellsOnLine.Count);
        line.sortingLayerName = "Foreground";
        line.SetWidth (0.05f, 0.05f);
        line.SetColors (Color.red, Color.red);

        Vector3[] position = new Vector3[cellsOnLine.Count];
        for (int i = 0; i < cellsOnLine.Count; i++)
        {
            Cell cell = cellsOnLine[i];
            Vector3 pointPos = cellsPos[cell.row, cell.col];
            position[i] = pointPos;
        }
        line.SetPositions (position);
    }


    public void HighLightSelectedCard ()
    {
        if (firstSelectCell == null)
            return;
        if (selectedGameObject)
            selectedGameObject.transform.GetChild(0).GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
        GameObject cell1;
        cells.TryGetValue (BaseUtil.GetCellId (firstSelectCell, row, col), out cell1);
        if (!cell1)
            return;
        Transform cel1Trans = cell1.transform;
        if (!cel1Trans)
            return;
        selectedGameObject = cel1Trans.gameObject;
        selectedGameObject.transform.GetChild(0).GetComponent<SpriteRenderer> ().color = colorPress;
        if (secondSelectCell != null && secondSelectCell != firstSelectCell)
        {
            GameObject cell2;
            cells.TryGetValue (BaseUtil.GetCellId (secondSelectCell, row, col), out cell2);
            cell2.transform.GetChild(0).GetComponent<SpriteRenderer> ().color = colorPress;            
        }
    }

    public void SwapCard (Cell c1, Cell c2)
    {
        GameObject gameObjectC1;
        cells.TryGetValue (BaseUtil.GetCellId (c1, row, col), out gameObjectC1);
        int cardTypeC1 = gameObjectC1.GetComponent<ItemInfo> ().type;

        GameObject gameObjectC2;
        cells.TryGetValue (BaseUtil.GetCellId (c2, row, col), out gameObjectC2);
        int cardTypeC2 = gameObjectC2.GetComponent<ItemInfo> ().type;

        //Sprite sprite1 = SpriteCollection.GetCollection (Card_Sprites).GetSprite ("item" + cardTypeC2);
        Sprite sprite1 = ThemeResource.Instance.CurrentTheme.GetSprite(cardTypeC2);
        gameObjectC1.GetComponent<SpriteRenderer>().sprite = sprite1;
        gameObjectC1.GetComponent<ItemInfo> ().type = cardTypeC2;

        //Sprite sprite2 = SpriteCollection.GetCollection (Card_Sprites).GetSprite ("item" + cardTypeC1);
        Sprite sprite2 = ThemeResource.Instance.CurrentTheme.GetSprite(cardTypeC1);
        gameObjectC2.GetComponent<SpriteRenderer>().sprite = sprite2;
        gameObjectC2.GetComponent<ItemInfo> ().type = cardTypeC1;
    }

    private Vector3[,] CalCellPos (int rowNum, int colNum, float minXPos, float minYPos, float cellWidth, float cellHeight)
    {
        Vector3[,] cellsPos = new Vector3[rowNum, colNum];
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                cellsPos[i, j] = new Vector3(0, 0, 0);
                cellsPos[i, j].x = minXPos + j * cellWidth + cellWidth / 2 - cellWidth / 2;
                cellsPos[i, j].y = minYPos + i * cellHeight + cellHeight / 2 - cellHeight / 2;
                cellsPos[i, j].z = (i * colNum) - j;
            }
        }
        return cellsPos;
    }

    private void CreateCardBoard ()
    {
        if (cells.Count > 0)
        {
            FillCardsToCells ();
        } else
        {
            CreateCells ();
        }
        transform.position = new Vector3 (0, transform.position.y, transform.position.z);
        //transform.DOMove (Vector3.zero, 1f);
    }

    private void FillCardsToCells ()
    {
        for (int i = 1; i < rowNum - 1; i++)
        {
            for (int j = 1; j < colNum - 1; j++)
            {
                FillCardToCell (matrix[i,j], new Cell (i, j));
            }
        }        
    }

    private void FillCardToCell (int cardType,Cell cell)
    {
        int cellId = BaseUtil.GetCellId (cell, row, col);
        GameObject cellGO;
        cells.TryGetValue (cellId, out cellGO);
        if (!cellGO)
            return;
        cellGO.GetComponent<ItemInfo> ().type = cardType;
        //Sprite sprite = SpriteCollection.GetCollection (Card_Sprites).GetSprite ("item" + cardType);
        Sprite sprite = ThemeResource.Instance.CurrentTheme.GetSprite(cardType);
        cellGO.GetComponent<SpriteRenderer> ().sprite = sprite;   
        cellGO.SetActive (cardType > 0 ? true : false);
        cellGO.transform.localEulerAngles = Vector3.zero;
        cellGO.transform.GetChild(0).GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);            
    }
    private void CreateCells ()
    {
        for (int i = 1; i < rowNum - 1; i++)
        {
            for (int j = 1; j < colNum - 1; j++)
            {
                AddCardToCell (matrix[i,j], new Cell (i, j), cellWidth, cellHeight);
            }
        }
    }

    private void AddCardToCell (int cardType,Cell cell,float width,float height)
    {
        // if (cardType == 0)
        //     return;
        Vector3 p = cellsPos[cell.row, cell.col];
        GameObject g = Instantiate(prefabPikachu) as GameObject;
        g.GetComponent<ItemInfo> ().type = cardType;
        g.transform.parent = this.transform;
        g.transform.position = p;
        g.name = cell.row + "_" + cell.col;
        // Sprite sprite = Resources.Load("Images/item/item" + cardType, typeof(Sprite)) as Sprite;
  
        //Sprite sprite = SpriteCollection.GetCollection (Card_Sprites).GetSprite ("item" + cardType);
        Sprite sprite = ThemeResource.Instance.CurrentTheme.GetSprite(cardType);
        g.GetComponent<SpriteRenderer>().sprite = sprite;
        Debug.LogWarning ("cardType: " + cardType);
        g.transform.localScale = new Vector3(Mathf.Abs(width * 1.0f / sprite.bounds.size.x), Mathf.Abs (- height * 1.0f / sprite.bounds.size.y), 1) * 1.09f;

        int cellId = BaseUtil.GetCellId (cell, row, col);
        cells.Add (cellId, g);
        g.SetActive (cardType > 0 ? true : false);
    }

    public void UpdateCountDownBar (float fillAmount)
    {
		if (countdownText != null && gameManager != null) {
            countdownText.text = string.Format("{0:D2}:{1:D2}", (int)(gameManager.remainTime / 60), (int)(gameManager.remainTime % 60));
        }
        countDownBar.fillAmount = fillAmount;
    }
    public void UpdateGuideText (string text)
    {
        //guideText.text = text;
    }


    public void ShowLoseDialog ()
    {
        //newScoreTxt.text = "Your Score: " + gameManager.score;
        //int bestScore = PlayerPrefs.GetInt (gameManager.gameMode.ToString () + gameManager.difficultLevel.ToString ());
        //bestScoreTxt.text = "Your Best Score: " + (gameManager.score > bestScore ? gameManager.score : bestScore);
        //loseDialog.SetActive (true);

        Media.Show(Media.Type.Interstitial, null, null);

        HUD1.Instance.ShowLosePopup();
        Settings.HasSave = 0;
        PlayerPrefs.Save();
    }

    public void SearchPair ()
    {
        if (gameManager.SearchNum <= 0) {
            if (Media.RewardedLoaded)
            {
                GameManager.Instance.isPause = true;
                Media.Show(Media.Type.Rewarded, () =>
                {
                    Search();
                    GameManager.Instance.isPause = false;
                });
            }
            else
            {
                if (Media.InterstitialLoaded)
                {
                    GameManager.Instance.isPause = true;
                    Media.Show(Media.Type.Interstitial, () =>
                    {
                        Search();
                        GameManager.Instance.isPause = false;
                    });
                }
                else
                {
                    HUD1.Instance.ShowYesNoPopup("Yes","Ads not load", () => {
                        GameManager.Instance.isPause = false;
                    }, () =>
                    {
                        GameManager.Instance.isPause = false;
                    });
                }
            }
            Analytics.LogEvent("Player watch ads to Search");
        }
        if (gameManager.SearchNum > 0)
        {
            Search();
        }
        
    }

    private void Search()
    {
        if (gameManager.currentPairCellIds.Count > 0 && !hasHint)
        {
            KeyValuePair<int, int> firstPair = gameManager.currentPairCellIds.FirstOrDefault();
            Cell firstCell = BaseUtil.GetCellById(firstPair.Key, row, col);
            Cell secondCell = BaseUtil.GetCellById(firstPair.Value, row, col);
            Vector3 firstTargetPos = cellsPos[firstCell.row, firstCell.col];
            Vector3 secondTargetPos = cellsPos[secondCell.row, secondCell.col];
            firstTarget.transform.position = new Vector3(firstTargetPos.x, firstTargetPos.y, firstTarget.transform.position.z);
            secondTarget.transform.position = new Vector3(secondTargetPos.x, secondTargetPos.y, secondTarget.transform.position.z);

            firstTarget.SetActive(true);
            secondTarget.SetActive(true);

            firstTarget.GetComponent<SpriteRenderer>().DOKill();
            secondTarget.GetComponent<SpriteRenderer>().DOKill();
            firstTarget.GetComponent<SpriteRenderer>().color = Color.white;
            secondTarget.GetComponent<SpriteRenderer>().color = Color.white;
            firstTarget.GetComponent<SpriteRenderer>().DOFade(0.5f, 0.2f).SetLoops(-1, LoopType.Yoyo);
            secondTarget.GetComponent<SpriteRenderer>().DOFade(0.5f, 0.2f).SetLoops(-1, LoopType.Yoyo);
            hasHint = true;

            gameManager.decreaseSearchNum();
        }
    }

    public void addTime(int time)
    {
        if (gameManager.TimeNum > 0)
        {
            gameManager.TimeNum--;
            gameManager.remainTime += time;
            gameManager.remainTime = Mathf.Clamp(gameManager.remainTime, 0, gameManager.matchTime);
            gameManager.mapUI.UpdateCountDownBar(gameManager.remainTime / gameManager.matchTime);
        }

    }

    public void Shuffe()
    {
        if (gameManager.ShuffeNum > 0)
        {
            gameManager.ShuffeNum--;
            gameManager.ResetMap();
        }
    }

    public void HighlightForSearch (Cell cell)
    {
        // fir
    }

    public void OnHomeClick ()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        Application.LoadLevel ("Home");
    }

    private void RegisterListenEvent ()
    {
        
    }

    public void OnChangeTheme()
    {
        HUD1.Instance.ShowChangeTheme();
    }

    public void ChangeTheme()
    {
        ItemInfo[] obj = FindObjectsOfType<ItemInfo>();
        foreach (ItemInfo item in obj)
        {
            //Sprite sprite = SpriteCollection.GetCollection(Card_Sprites).GetSprite("item" + item.type);
            Sprite sprite = ThemeResource.Instance.CurrentTheme.GetSprite(item.type);
            item.GetComponent<SpriteRenderer>().sprite = sprite;
        } 
    }

}
