using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public abstract class HUD : MonoBehaviour, IFrameListener
{
	[Space()]
	private SuperImage image;
	protected Stack<IFrame> frames = new Stack<IFrame>();
	private Color normalColor;

	public Stack<IFrame> GetFrames() {
		return frames;
	} 

	void Awake()
	{
		image = GetComponent<SuperImage>();
		normalColor = image.color;
		OnAwake();
	}

	protected virtual void OnAwake() {

	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (Current != null && Current.Dismissable)
			{
				Current.Dismiss(true);
			}
		}
		OnUpdate();
	}

	protected virtual void OnUpdate() {

	} 		

	#region Show

	public void Show(MonoFrame frame, bool animated, object data = null, bool dismissCurrent = true, bool pauseCurrent = true)
	{
		SwitchAllScroll(frame, false);
		ResetAllScroll(frame);
		if (frames.Contains(frame))
		{
			Debug.LogError("Frame already shown");
			return;
		}

		if (Current != null && Current.IsTopFrame) {
			return;
		}

		image.enabled = true;
		image.UpdateVertices (null, Vector4.zero);

		if (dismissCurrent && Current != null && Current.Dismissable)
		{
			var current = frames.Pop();
			current.Listener = null;
			current.Dismiss(true);
		}
		else
		{
			var current = Current;
			if (current != null && pauseCurrent)
			{
				current.Pause(true);
			}
		}
		frames.Push(frame);
		frame.Listener = this;
		frame.Show(data, animated);
	}
	#endregion

	public void Pop()
	{
		if (Current != null && Current.Dismissable)
		{
			Current.Dismiss(true);
		}
	}

	public void DismissAllFrames()
	{
		while (frames.Count > 0)
		{
			var frame = frames.Pop();
			frame.Listener = null;
			frame.Dismiss(false);
		}

        if (image != null)
            image.enabled = false;
	}

	public IFrame Current
	{
		get { return frames.Count > 0 ? frames.Peek() : null; }
	}

	#region IFrameListener

	public void OnShown(MonoFrame frame)
	{
		SwitchAllScroll(frame, true);
		image.UpdateVertices (frame.solidZone, frame.solidZoneMargin);
	}

	private void ResetAllScroll(MonoFrame frame) {
		var cpns = frame.GetComponentsInChildren<ScrollRect>();
		foreach (var item in cpns)
		{
			item.verticalNormalizedPosition = 1;
		}
	}

	private void SwitchAllScroll(MonoFrame frame, bool enable) {
		var cpns = frame.GetComponentsInChildren<ScrollRect>();
		foreach (var item in cpns)
		{
			item.enabled = enable;
		}
	}

	public void OnDismissed(MonoFrame frame)
	{

		frame.Listener = null;
		if (Current == frame)
		{
			frames.Pop();
		}

		if (Current != null)
		{
			image.UpdateVertices(null, Vector4.zero);
			Current.Resume(true);
		}

		if (frames.Count == 0)
		{
			image.enabled = false;
		}
	}

	public void OnPaused(MonoFrame frame)
	{

	}

	public void OnResumed(MonoFrame frane)
	{
		image.UpdateVertices(frane.solidZone, frane.solidZoneMargin);
	}

	#endregion
}

public class MonoFrame : MonoBehaviour, IFrame 
{
	public RectTransform solidZone;
	public Vector4 solidZoneMargin;

	#region IFrame

	public bool dismissable = true;
	public bool Dismissable {
		get { return dismissable; }
		set { dismissable = value; }
	}

	public bool isTopFrame = false;
	public bool IsTopFrame {
		get { return isTopFrame; }
		set { isTopFrame = value; }
	}

	public virtual void Show (object data, bool animated)
	{
		gameObject.SetActive (true);
		if (animated) {
			StartCoroutine (AnimateShow (0.5f, () => { //0.5f
				if (Listener != null)
					Listener.OnShown (this);
			}));
		} else {
			if (Listener != null)
				Listener.OnShown (this);	
		}
	}

	public virtual void Dismiss (bool animated)
	{
		if (animated) {
			StartCoroutine (AnimateDismiss (0.0f, () => {//0.5f
				if (Listener != null)
					Listener.OnDismissed (this);
			}));
		} else {
			gameObject.SetActive (false);
			if (Listener != null)
				Listener.OnDismissed (this);
		}
	}

	public virtual void Pause (bool animated)
	{
		if (animated) {
			StartCoroutine (AnimateDismiss (0.0f, () => {
				if (Listener != null)
					Listener.OnPaused (this);
			}));
		} else {
			gameObject.SetActive (false);
			if (Listener != null)
				Listener.OnPaused (this);
		}
	}

	public virtual void Resume (bool animated)
	{
		if (gameObject.activeSelf)
			return;
		gameObject.SetActive (true);
		if (animated) {
			StartCoroutine (AnimateShow (0.5f, () => {
				if (Listener != null)
					Listener.OnResumed (this);
			}));
		} else {
			if (Listener != null)
				Listener.OnResumed (this);
		}
	}

	public IFrameListener Listener {
		get;
		set;
	}

	#endregion

	#region Animation

	IEnumerator AnimateShow (float duration, UnityAction listener)
	{
//		Globals.hud.hudAnim.FadeIn(this, ()=> listener.Invoke());

		//temp
		listener.Invoke();
		yield return null;
	}

	IEnumerator AnimateDismiss (float duration, UnityAction listener)
	{
		if (listener != null) {
			listener.Invoke ();
		}
		gameObject.SetActive (false);
		yield return null;
	}

	#endregion

	#region Button Callback

	public virtual void OnCloseButtonClicked ()
	{
		Dismiss (true);
	}

	#endregion
}

public interface IFrame
{
	bool Dismissable {
		get;
		set;
	}

	bool IsTopFrame {
		get;
		set;
	}

	void Show(object data, bool animated);

	void Dismiss(bool animated);

	void Pause(bool animated);

	void Resume(bool animated);

	IFrameListener Listener
	{
		get;
		set;
	}
}

public interface IFrameListener
{
	void OnShown(MonoFrame frame);

	void OnDismissed(MonoFrame frame);

	void OnPaused(MonoFrame frame);

	void OnResumed(MonoFrame frane);

}
