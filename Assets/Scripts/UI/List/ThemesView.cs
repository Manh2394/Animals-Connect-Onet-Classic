using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemesView : View
{
    protected override List<object> GetData()
    {
        List<object> list = new List<object>();
        foreach (var item in ThemeResource.Instance.Themes)
        {
            list.Add(item);
        }
        return list;
    }
}
