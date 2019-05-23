using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class YesNoPopup : MonoFrame {

    private Action actionYes;
    private Action actionNo;
    public Text title;
    public Text yesText;

    public override void Show(object data, bool animated)
    {
        base.Show(data, animated);
        GameManager.Instance.isPause = true;
    }

    public void Yes()
    {
        if (actionYes != null)
        {
            actionYes.Invoke();
        }
        OnCloseButtonClicked();
    }

    public void No()
    {
        if (actionNo != null)
        {
            actionNo.Invoke();
        }
        OnCloseButtonClicked();
    }

    public void SetContent(string yesText, string title, Action actionYes, Action actionNo)
    {
        this.actionYes = actionYes;
        this.actionNo = actionNo;
        this.title.text = title;
        this.yesText.text = yesText;
    }

    public override void OnCloseButtonClicked()
    {
        base.OnCloseButtonClicked();
        actionYes = null;
        actionNo = null;
    }
}
