using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorUtils
{

    public static void HorizontalSeparator()
    {
        GUIStyle styleHR = new GUIStyle(GUI.skin.box);
        styleHR.stretchWidth = true;
        styleHR.fixedHeight = 2;
        GUILayout.Box("", styleHR);
    }

    [UnityEditor.MenuItem("Game/Clear Data", false, 22)]
    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

}
