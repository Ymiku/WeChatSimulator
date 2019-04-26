using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class TweetItem : ItemBaseInconsist {
	public ImageProxy head;
	public TextProxy mainBody;
	public RectTransform root;
	public ImageProxy[] pic;
	TweetData data;
	public override float SetData(object o)
	{
		base.SetData(o);
		data = o as TweetData;
		HeadSpriteUtils.Instance.SetHead (head,data.userId);
		float height = 0.0f;
		height -= root.anchoredPosition.y;
		height -= mainBody.rectTransform.anchoredPosition.y;
		mainBody.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,2000.0f);
		mainBody.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,mainBody.preferredHeight);
		height += mainBody.rectTransform.sizeDelta.y;


		return height;
	}
	public void OnClickHead()
	{
		UIManager.Instance.Push (new PersonalHomePageContext(data.userId));
	}
	public void OnClickDots()
	{
		
	}
}
