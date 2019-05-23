using UnityEngine;
using System.Collections;
using DG.Tweening;

public class LuckyWheel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int loopNum = Random.Range (20, 30);
		this.transform.DOLocalRotate (new Vector3 (0,0,360), 0.2f, RotateMode.FastBeyond360).SetLoops (loopNum, LoopType.Restart).OnComplete (() => {
			this.transform.DOLocalRotate (new Vector3 (0, 0, 360), 0.3f, RotateMode.FastBeyond360).SetLoops (3, LoopType.Restart);
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
