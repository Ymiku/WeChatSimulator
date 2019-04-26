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
			TweetData tweet = new TweetData ();
			tweet.id = a.tweet_id;
			tweet.userId = a.user_id;
            tweet.info = a.tweet_info;
			tweet.order = a.order;
			tweet.isSecret = a.is_secret;
			tweet.pics = new int[a.pic_array.Count];
			for (int m = 0; m < tweet.pics.Length;m++) {
				tweet.pics[m] = a.pic_array[m];
			}
			tweet.location = a.location;
			List<TweetData> tweets;
			if (ZoneManager.Instance.id2Tweet.TryGetValue (a.user_id,out tweets)) {

			} else {
				tweets = new List<TweetData> ();
				ZoneManager.Instance.id2Tweet.Add (a.user_id,tweets);
			}
			tweets.Add(tweet);
		}
	}
}
