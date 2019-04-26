using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class TweetItem : ItemBaseInconsist {
	public ImageProxy head;
	public TextProxy name;
	public TextProxy mainBody;
	public RectTransform root;
	public ImageProxy[] pic;
	public TextProxy likeNames;
	public TextProxy comment;
	TweetData data;
	public CanvasGroup dotsCanvas;
	public override float SetData(object o)
	{
		base.SetData(o);
		data = o as TweetData;
		HeadSpriteUtils.Instance.SetHead (head,data.userId);
		name.text = XMLSaver.saveData.GetAccountData (data.userId).GetAnyName();
		float height = 0.0f;
		height -= root.anchoredPosition.y;
		height -= mainBody.rectTransform.anchoredPosition.y;
		mainBody.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,2000.0f);
		mainBody.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,mainBody.preferredHeight);
		height += mainBody.rectTransform.sizeDelta.y;
		float picHeight = 0.0f;
		height += picHeight;
		likeNames.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,2000.0f);
		likeNames.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,likeNames.preferredHeight);
		height += likeNames.rectTransform.sizeDelta.y;
		comment.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,2000.0f);
		comment.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,comment.preferredHeight);
		height += comment.rectTransform.sizeDelta.y;

		return height;
	}
	public void OnClickHead()
	{
		UIManager.Instance.Push (new PersonalHomePageContext(data.userId));
	}
	RXIndex dotsRX = new RXIndex();
	void OnDisable()
	{
		FrostRX.End (dotsRX);
	}
	public void OnClickDots()
	{
		if (dotsCanvas.blocksRaycasts) {
			OnCloseDots ();
		} else {
			OnOpenDots ();
		}
	}
	public void OnOpenDots()
	{
		FrostRX.End (dotsRX);
		FrostRX.Start(this).Execute(()=>{dotsCanvas.blocksRaycasts=true;}).ExecuteUntil(()=>{dotsCanvas.alpha = Mathf.Lerp(dotsCanvas.alpha,1.01f,8.0f*Time.deltaTime);},()=>{return dotsCanvas.alpha>=1.0f;}).GetId(dotsRX);
	}
	public void OnCloseDots()
	{
		FrostRX.End (dotsRX);
		FrostRX.Start(this).Execute(()=>{dotsCanvas.blocksRaycasts=false;}).ExecuteUntil(()=>{dotsCanvas.alpha = Mathf.Lerp(dotsCanvas.alpha,-0.01f,8.0f*Time.deltaTime);},()=>{return dotsCanvas.alpha<=0.0f;}).GetId(dotsRX);
	}
}
