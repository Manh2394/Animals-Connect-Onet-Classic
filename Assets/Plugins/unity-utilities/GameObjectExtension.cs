using UnityEngine;

static public class GameObjectExtension
{
    /// <summary>
    /// Get or Add component
    // </summary>
    static public T GetOrAddComponent<T> (this GameObject go) where T: Component
    {
        T comp = go.GetComponent<T> ();
        if (comp == null) {
            comp = go.AddComponent<T> ();
        }
        return comp;
    }
}
