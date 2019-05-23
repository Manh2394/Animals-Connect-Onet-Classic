using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(menuName = "Theme", fileName = "Theme")]
public class Theme : ScriptableObject {

    public string id;
    public string name;
    public Sprite avatar;
    public List<Sprite> sprites = new List<Sprite>();

    public int Index
    {
        get; set;
    }

    public bool unlock
    {
        get { return PlayerPrefsX.GetBool(id + "unlock"); }
        set { PlayerPrefsX.SetBool(id + "unlock", value); }
    }

    public void Unlock()
    {
        unlock = true;
    }

    [ContextMenu("Generate ID (if needed)")]
    public void GenerateID()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }

    public Sprite GetSprite(int index)
    {
        if (index >= 0 && index < sprites.Count)
            return sprites[index];

        return null;
    }

    public Sprite GetSprite(string spriteName)
    {
        foreach (Sprite sprite in sprites)
        {
            if (sprite != null && sprite.name == spriteName)
                return sprite;
        }
        return null;
    }

    public void Initialize()
    {

    }

#if UNITY_EDITOR
    public void Unload()
    {

    }
#endif
}
