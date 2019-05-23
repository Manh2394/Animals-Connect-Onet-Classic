using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings {

    public static readonly string SEARCH_NUM = "SEARCH_NUM";
    public static readonly string SHUFFE_NUM = "SHUFFE_NUM";
    public static readonly string TIME_NUM = "TIME_NUM";
    public static readonly string HAS_SAVE = "HAS_SAVE";
    public static readonly string GAME_MODE = "GAME_MODE";
    public static readonly string DIFFICULT_LEVER = "DIFFICULT_LEVER";
    public static readonly string LEVEL = "LEVEL";
    public static readonly string SCORE = "SCORE";
    public static readonly string REMAIN_TIME = "REMAIN_TIME";
    public static readonly string PAIR_NUM = "PAIR_NUM";
    public static readonly string MATRIX = "MATRIX";

    public static readonly string THEME = "THEME";
    public static readonly string THEME_PATH = "CardSprites";
    public static readonly string THEME_PATH2 = "CardSprites2";

    public static readonly string CURRENT_THEME = "CURRENT_THEME";

    public static readonly string THEME_CHANGED = "ThemeChanged";

    public static int SaveMode
    {
        get; set;
    }

    public static bool ThemeChanged
    {
        get
        {
            return PlayerPrefsX.GetBool(THEME_CHANGED);
        }

        set
        {
            PlayerPrefsX.SetBool(THEME_CHANGED, value);
        }
    }

    public static int[] Matrix
    {
        get
        {
            return PlayerPrefsX.GetIntArray(MATRIX + SaveMode);
        }

        set
        {
            PlayerPrefsX.SetIntArray(MATRIX + SaveMode, value);
        }
    }

    public static int SearchNum
    {
        get
        {
            return PlayerPrefs.GetInt(SEARCH_NUM + SaveMode);
        }

        set
        {
            PlayerPrefs.SetInt(SEARCH_NUM + SaveMode, value);
        }
    }

    public static int ShuffeNum
    {
        get
        {
            return PlayerPrefs.GetInt(SHUFFE_NUM + SaveMode);
        }

        set
        {
            PlayerPrefs.SetInt(SHUFFE_NUM + SaveMode, value);
        }
    }

    public static int TimeNum
    {
        get
        {
            return PlayerPrefs.GetInt(TIME_NUM + SaveMode);
        }

        set
        {
            PlayerPrefs.SetInt(TIME_NUM + SaveMode, value);
        }
    }

    public static int HasSave
    {
        get
        {
            return PlayerPrefs.GetInt(HAS_SAVE + SaveMode);
        }

        set
        {
            PlayerPrefs.SetInt(HAS_SAVE + SaveMode, value);
        }
    }
    public static int GameMode
    {
        get
        {
            return PlayerPrefs.GetInt(GAME_MODE + SaveMode);
        }

        set
        {
            PlayerPrefs.SetInt(GAME_MODE + SaveMode, value);
        }
    }

    public static int DifficultLevel
    {
        get
        {
            return PlayerPrefs.GetInt(DIFFICULT_LEVER + SaveMode);
        }

        set
        {
            PlayerPrefs.SetInt(DIFFICULT_LEVER + SaveMode, value);
        }
    }

    public static int Level
    {
        get
        {
            return PlayerPrefs.GetInt(LEVEL + SaveMode);
        }

        set
        {
            PlayerPrefs.SetInt(LEVEL + SaveMode, value);
        }
    }
    public static int Score
    {
        get
        {
            return PlayerPrefs.GetInt(SCORE + SaveMode);
        }

        set
        {
            PlayerPrefs.SetInt(SCORE + SaveMode, value);
        }
    }

    public static int RemainTime
    {
        get
        {
            return PlayerPrefs.GetInt(REMAIN_TIME + SaveMode);
        }

        set
        {
            PlayerPrefs.SetInt(REMAIN_TIME + SaveMode, value);
        }
    }

    public static int PairNum
    {
        get
        {
            return PlayerPrefs.GetInt(PAIR_NUM + SaveMode);
        }

        set
        {
            PlayerPrefs.SetInt(PAIR_NUM + SaveMode, value);
        }
    }

    public static string Theme
    {
        get
        {
            return PlayerPrefs.GetString(THEME + SaveMode, THEME_PATH);
        }

        set
        {
            PlayerPrefs.SetString(THEME + SaveMode, value);
        }
    }

    public static string CurrentTheme
    {
        get
        {
            return PlayerPrefs.GetString(CURRENT_THEME + SaveMode, null);
        }

        set
        {
            PlayerPrefs.SetString(CURRENT_THEME + SaveMode, value);
        }
    }

    public static void AddSearch(int value)
    {
        SearchNum += value;
    }

    public static void SpendSearch(int value)
    {
        SearchNum -= value;
    }

    public static void AddShuffe(int value)
    {
        ShuffeNum += value;
    }

    public static void SpendShuffe(int value)
    {
        ShuffeNum -= value;
    }

    public static void AddTime(int value)
    {
        TimeNum += value;
    }

    public static void SpendTime(int value)
    {
        TimeNum -= value;
    }

}
