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
	public TextProxy locationText;
	public TextProxy likesText;
	public TextProxy commentText;
	public RectTransform heartIcon;
    public RectTransform downPanelLine;
    public RectTransform downPanel;
    public RectTransform picGroup;
	TweetData data;
	public CanvasGroup dotsCanvas;

    BinaryList<CommentData> comments = new BinaryList<CommentData>();
    BinaryList<CommentData> likes = new BinaryList<CommentData>();
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    public override float SetData(object o)
	{
		base.SetData(o);
		data = o as TweetData;
		HeadSpriteUtils.Instance.SetHead (head,data.userId);
		userName.text = XMLSaver.saveData.GetAccountData (data.userId).GetAnyName();
		float itemHeight = 0.0f;
		itemHeight -= root.anchoredPosition.y;
		itemHeight -= mainBody.rectTransform.anchoredPosition.y;
		mainBody.text = data.info;
		mainBody.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,2000.0f);
		mainBody.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,mainBody.preferredHeight);
		itemHeight += mainBody.rectTransform.sizeDelta.y;
        itemHeight += 26.0f;

        picGroup.anchoredPosition = new Vector2(picGroup.anchoredPosition.x,-itemHeight+20.0f);
		for (int i = 0; i < pic.Length; i++) {
			pic [i].gameObject.SetActive (false);
		}
        mainPic.gameObject.SetActive(false);
        if (data.pics.Length == 1)
        {
            mainPic.sprite = ZoneManager.Instance.picArray[data.pics[0]].pic;
            mainPic.sizeDelta = Utils.CalSpriteDisplaySize(pic[0].sprite.bounds.size, new Vector2(750.0f, 535.0f));
            itemHeight += mainPic.sizeDelta.y;
            itemHeight += 30.0f;
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
                itemHeight += 260.0f;
            }
            else
            {
                itemHeight += 530.0f;
            }
            itemHeight += 30.0f;
        }

        locationText.anchoredPosition = new Vector2(locationText.anchoredPosition.x, -itemHeight + 20.0f);
		locationText.text = data.location;
        itemHeight += 70.0f;

        comments.Clear();
        likes.Clear();
        if(ZoneManager.Instance.id2Comment.ContainsKey(data.userId))
        for (int i = 0; i < ZoneManager.Instance.id2Comment[data.userId].Count; i++)
        {
            CommentData commentData = ZoneManager.Instance.id2Comment[data.userId][i];
            if (commentData.commentType != CommentType.Tweet || commentData.id != data.id)
                continue;
            if (string.IsNullOrEmpty(commentData.info))
            {
                likes.Add(commentData);
            }
            else
            {
                comments.Add(commentData);
            }
        }

		if (likes.Count == 0 && comments.Count == 0) {
			downPanel.gameObject.SetActive (false);
			itemHeight += 10.0f;
			height = itemHeight;
			dotsCanvas.alpha = 0.0f;
			dotsCanvas.blocksRaycasts = false;
			comments.Clear();
			likes.Clear();
			sb.Length = 0;
			return itemHeight;
		}
		downPanel.gameObject.SetActive (true);
        downPanel.anchoredPosition = new Vector2(downPanel.anchoredPosition.x, -itemHeight + 20.0f);
        sb.Length = 0;

		float addHeight = 0.0f;
		addHeight += 25.0f;
        if (likes.Count != 0)
            sb.Append(XMLSaver.saveData.GetAccountData(likes[0].userId).GetAnyName());
        for (int i = 1; i < likes.Count; i++)
        {
            sb.Append("，"+XMLSaver.saveData.GetAccountData(likes[0].userId).GetAnyName());
        }
		if (likes.Count == 0) {
			likesText.gameObject.SetActive (false);
			heartIcon.gameObject.SetActive (false);
		} else {
			likesText.text = sb.ToString();
			//likeNames.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,2000.0f);
			likesText.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,likesText.preferredHeight);
			likesText.gameObject.SetActive (true);
			heartIcon.gameObject.SetActive (true);
			addHeight += (90.0f-40.0f+likesText.height);
		}
        
        sb.Length = 0;
        if (comments.Count != 0)
            sb.Append(XMLSaver.saveData.GetAccountData(comments[0].userId).GetAnyName()+ "：<color=black>"+comments[0].info+ "</color>");
        for (int i = 1; i < likes.Count; i++)
        {
            sb.Append("\n");
            sb.Append(XMLSaver.saveData.GetAccountData(comments[i].userId).GetAnyName() + "：<color=black>" + comments[i].info + "</color>");
        }
		if (comments.Count == 0) {
			commentText.gameObject.SetActive (false);
		} else {
			commentText.text = sb.ToString();
			commentText.anchoredPosition = new Vector2(commentText.anchoredPosition.x, - addHeight -20.0f);
			//comment.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,2000.0f);
			commentText.rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,commentText.preferredHeight);
			commentText.gameObject.SetActive (true);
			addHeight+=(40.0f+commentText.height);
		}
        
		downPanel.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,addHeight);
		itemHeight += addHeight;

        itemHeight += 10.0f;
        height = itemHeight;
        dotsCanvas.alpha = 0.0f;
        dotsCanvas.blocksRaycasts = false;
        comments.Clear();
        likes.Clear();
        sb.Length = 0;
		return itemHeight;
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
		FrostRX.Start(this).Execute(()=>{dotsCanvas.blocksRaycasts=true;}).ExecuteUntil(()=>{dotsCanvas.alpha = Mathf.Lerp(dotsCanvas.alpha,1.01f,16.0f*Time.deltaTime);},()=>{return dotsCanvas.alpha>=1.0f;}).GetId(dotsRX);
	}
	public void OnCloseDots()
	{
		FrostRX.End (dotsRX);
		FrostRX.Start(this).Execute(()=>{dotsCanvas.blocksRaycasts=false;}).ExecuteUntil(()=>{dotsCanvas.alpha = Mathf.Lerp(dotsCanvas.alpha,-0.01f,16.0f*Time.deltaTime);},()=>{return dotsCanvas.alpha<=0.0f;}).GetId(dotsRX);
	}
    public void OnClickLike()
    {
        XMLSaver.saveData.AddComment(CommentType.Tweet,data.id,null);
        SetData(data);
    }
}
