using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeThemePopup : MonoFrame {

    private void OnEnable()
    {
        GameManager.Instance.isPause = true;
    }

    private void OnDisable()
    {
        GameManager.Instance.isPause = false;
    }
}
