using static_data;
using System.Collections.Generic;
using UnityEngine;

public static class StaticDataTweet
{
	public static TWEET_ARRAY Info;

	public static void Init()
	{
		Info = StaticDataLoader.ReadOneDataConfig<TWEET_ARRAY>("Tweet");
		TransToTweetData ();
	}

	public static void TransToTweetData()
	{
		for (int i = 0; i < Info.items.Count; i++)
		{
			TWEET a = Info.items [i];
			TweetData Tweet = new TweetData ();
			Tweet.id = a.tweet_id;
			Tweet.userId = a.user_id;
			Tweet.order = a.order;
			Tweet.isSecret = a.is_secret;
			Tweet.pics = new int[a.pic_array.Count];
			for (int m = 0; m < Tweet.pics.Length;m++) {
				Tweet.pics[m] = a.pic_array[m];
			}
			List<TweetData> Tweets;
			if (ZoneManager.Instance.id2Tweet.TryGetValue (a.user_id,out Tweets)) {

			} else {
				Tweets = new List<TweetData> ();
				ZoneManager.Instance.id2Tweet.Add (a.user_id,Tweets);
			}
			Tweets.Add(Tweet);
		}
	}
}
