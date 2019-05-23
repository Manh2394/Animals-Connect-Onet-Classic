using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour {

    public GameObject itemPrefab;
    public Transform parent;
    protected List<Item> items = new List<Item>();

    protected abstract List<object> GetData();

    private void OnEnable()
    {
        StartCoroutine(Gen());
    }

    protected virtual IEnumerator Gen()
    {
        Reset();
        yield return new WaitForEndOfFrame();
        if (GetData() != null)
        {
            foreach (var item in GetData())
            {
                GameObject obj = Instantiate(itemPrefab, parent);
                Item itemComponent = obj.GetComponent<Item>();
                itemComponent.UpdateUI(item);
                obj.SetActive(true);
                items.Add(itemComponent);
            }
        }
    }

    protected void UpdateUI(object data)
    {
        foreach (var item in items)
        {
            item.UpdateUI(item);
        }
    }

    protected virtual void Reset()
    {
        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
        items = new List<Item>();
    }
}
