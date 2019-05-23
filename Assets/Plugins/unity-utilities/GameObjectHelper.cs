using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameObjectHelper
{
    public static bool IsPrefab (Object o)
    {
        var go = o as GameObject;
        if (go != null) {
            return IsPrefab (go);
        }
        var comp = o as Component;
        if (comp != null) {
            return IsPrefab (comp);
        }

        return false;
    }

    public static bool IsPrefab (GameObject go)
    {
        return go.transform.hideFlags == HideFlags.HideInHierarchy;
    }

    public static bool IsPrefab (Component comp)
    {
        return comp.gameObject.transform.hideFlags == HideFlags.HideInHierarchy;
    }

    public static GameObject Find (string name)
    {
        Object[] objects = Resources.FindObjectsOfTypeAll (typeof(GameObject));
        return objects.First (go => go.name == name && !IsPrefab (go as GameObject)) as GameObject;
    }

    public static T FindObjectOfType<T> () where T:Object
    {
        Object[] objects = Resources.FindObjectsOfTypeAll (typeof(T));
        return objects.First (go => !IsPrefab (go)) as T;
    }

    public static GameObject FindObjectOfType (System.Type T)
    {
        Object[] objects = Resources.FindObjectsOfTypeAll (T);
        return objects.First (go => !IsPrefab (go)) as GameObject;
    }

    public static T[] FindObjectsOfType<T> () where T:Object
    {
        Object[] objects = Resources.FindObjectsOfTypeAll (typeof(T));
        return objects.Where (go => !IsPrefab (go)).Select (go => go as T).ToArray ();
    }

    public static GameObject[] FindObjectsOfType (System.Type T)
    {
        Object[] objects = Resources.FindObjectsOfTypeAll (T);
        return objects.Where (go => !IsPrefab (go)).Select (go => go as GameObject).ToArray ();
    }

    public static GameObject FindGameObjectWithTag (string tag)
    {
        Object[] objects = Resources.FindObjectsOfTypeAll (typeof(GameObject));
        return objects.Select (go => go as GameObject).First(go => go.CompareTag(tag) && !IsPrefab(go));
    }

    public static GameObject[] FindGameObjectsWithTag (string tag)
    {
        Object[] objects = Resources.FindObjectsOfTypeAll (typeof(GameObject));
        return objects.Select (go => go as GameObject).Where(go => go.CompareTag(tag) && !IsPrefab(go)).ToArray();
    }

}
