using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ThemeResource", fileName = "ThemeResource")]
public class ThemeResource : ScriptableObject
{
    private static ThemeResource instance;

    [SerializeField] private List<Theme> themes;
    public List<Theme> Themes
    {
        get
        {
            return themes;
        }
    }

    public static ThemeResource Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<ThemeResource>("ThemeResource");
            }
            return instance;
        }
    }


    public int GetIndex(Theme theme)
    {
        return themes.IndexOf(theme);
    }

    public void Initialize()
    {
        if (!themes[0].unlock)
        {
            themes[0].Unlock();
        }

        for (int i = 0; i < themes.Count; i++)
        {
            themes[i].Index = i;
            themes[i].Initialize();
        }
    }

#if UNITY_EDITOR
    public static void Unload()
    {
        foreach (var item in Instance.themes)
        {
            item.Unload();
            Resources.UnloadAsset(item);
        }
        Resources.UnloadAsset(instance);
    }
#endif

    public Theme this[int index]
    {
        get
        {
            index = Mathf.Clamp(index, 0, ThemeCount - 1);
            return themes[index];
        }
    }

    public Theme this[string id]
    {
        get
        {
            return themes.Find((Theme t) => t.id == id);
        }
    }

    public Theme CurrentTheme
    {
        get
        {
            if (Settings.CurrentTheme == null || Settings.CurrentTheme == "")
            {
                return this[0];
            }
            return this[Settings.CurrentTheme];
        }

        set
        {
            Settings.CurrentTheme = value.id;
        }
    }

    public int ThemeCount
    {
        get { return themes.Count; }
    }

    [ContextMenu("Generate Level ID (if needed)")]
    public void GenerateID()
    {
        foreach (var i in themes)
        {
            i.GenerateID();
        }
    }
}
