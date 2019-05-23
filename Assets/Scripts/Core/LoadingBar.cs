using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {
	private Image progressImg;
    void Awake ()
    {
        progressImg = transform.Find ("Progress").gameObject.GetComponent<Image> ();
    }
    public void FillAmount (float amount)
    {
        progressImg.fillAmount = Mathf.Max (progressImg.fillAmount, amount);
    }
}
