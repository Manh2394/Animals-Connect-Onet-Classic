using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
using LOT.Core;

public class BaseScene : MonoBehaviour, IScene {

	public void OnShowDialog (GameObject dialog)
	{
		StartCoroutine (ShowDialog (dialog));
	}
	public void OnDialogClose (GameObject dialog)
	{
		StartCoroutine (CloseDialog (dialog, null));
	}
    public IEnumerator CloseDialog (GameObject dialog, UnityAction callback)
    {
        var tween = dialog.transform.DOScale (Vector3.zero, 0.5f);
        tween.Play ();
        yield return tween.WaitForCompletion ();
        dialog.SetActive (false);        
        if (callback != null)
            callback.Invoke ();
    }
	public IEnumerator ShowDialog (GameObject dialog)
    {
        dialog.SetActive (true);
        dialog.transform.localScale = Vector3.zero;
        yield return null;
        dialog.transform.DOScale (Vector3.one, 0.5f);
        
    }
    public virtual IEnumerator Initialize(GeneralOptions options)
    {
        throw new NotImplementedException();
    }
}
