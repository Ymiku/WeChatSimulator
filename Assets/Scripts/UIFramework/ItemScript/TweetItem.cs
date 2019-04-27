using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class TweetItem : ItemBaseInconsist {
	public ImageProxy head;
	public TextProxy userName;
	public TextProxy mainBody;
	public RectTransform root;
    public ImageProxy mainPic;
	public ImageProxy[] pic;
	public TextProxy location;
	public TextProxy likeNames;
	public TextProxy comment;
    public RectTransform commentLine;
	TweetData data;
	public CanvasGroup dotsCanvas;
	public override float SetData(object o)
	{
		base.SetData(o);
		data = o as TweetData;
		HeadSpriteUtils.Instance.SetHead (head,data.userId);
		userName.text = XMLSaver.saveData.GetAccountData (data.userId).GetAnyName();
		float height = 0.0f;
		height -= root.anchoredPosition.y;
		height -= mainBody.rectTransform.anchoredPosition.y;
		mainBody.text = data.info;
		mainBody.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,2000.0f);
		mainBody.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,mainBody.preferredHeight);
		height += mainBody.rectTransform.sizeDelta.y;
        height += 26.0f;
		for (int i = 0; i < pic.Length; i++) {
			pic [i].gameObject.SetActive (false);
		}
        mainPic.gameObject.SetActive(false);
        if (data.pics.Length == 1)
        {
            mainPic.sprite = ZoneManager.Instance.picArray[data.pics[0]].pic;
            mainPic.sizeDelta = Utils.CalSpriteDisplaySize(pic[0].sprite.bounds.size, new Vector2(750.0f, 535.0f));
            mainPic.gameObject.SetActive(true);
        }
        else if(data.pics.Length>=2)
        {
            for (int i = 0; i < Mathf.Min(data.pics.Length,6); i++)
            {
                pic[i].sprite = ZoneManager.Instance.picArray[data.pics[i]].pic;
                pic[i].gameObject.SetActive(true);
            }
            mainPic.sprite = ZoneManager.Instance.picArray[data.pics[0]].pic;
            mainPic.sizeDelta = Utils.CalSpriteDisplaySize(pic[0].sprite.bounds.size, new Vector2(750.0f, 535.0f));
            mainPic.gameObject.SetActive(true);
            if (data.pics.Length <= 3)
            {
                height += 260.0f;
            }
            else
            {
                height += 530.0f;
            }
            height += 30.0f;
        }
		location.text = data.location;
        height += 70.0f;

        height += 230.0f;
		likeNames.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,2000.0f);
		likeNames.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,likeNames.preferredHeight);
        float addHeight = (likeNames.rectTransform.sizeDelta.y - 39.1111f);
        height += addHeight;

        commentLine.anchoredPosition = new Vector2(commentLine.anchoredPosition.x, -113.62f-addHeight);
        comment.anchoredPosition = new Vector2(comment.anchoredPosition.x, -135.5f-addHeight);
		comment.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,2000.0f);
		comment.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,comment.preferredHeight);
		height += (comment.rectTransform.sizeDelta.y - 39.1111f);

        height += 10.0f;
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
