using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemScrollView : MonoBehaviour {

    public ScrollView scrollView;
    public string id;

	// Use this for initialization
    public virtual void OnClick(){
        scrollView.idSelect = id;
    }
	
}
