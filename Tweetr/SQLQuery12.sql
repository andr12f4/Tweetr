﻿SELECT Tweets.Id, Tweets.Tweet ,Tweets.DOB, Tweets.Tpublicity,Tweets.Likes, Tweets.customerId, Likes.UserID FROM Tweets Right JOIN Likes ON Tweets.Id = Likes.TweetId WHERE Tweets.Id = 1