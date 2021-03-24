using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetr.Models;

namespace Tweetr.Interfaces
{
    public interface ITweet
    {
        public void Create(Tweet tweet);
        public Tweet GetTweet(int id);
        public void UpdateTweet(int id, Tweet tweet);
        public void DeleteTweet(int id);
        public Dictionary<int,Tweet> GetAllPublicTweets();
        public Dictionary<int,Tweet> GetAllFriendsTweets(int userID);
        public Dictionary<int,Tweet> GetAllPrivateTweets(int UserID);

        public void AddLike(int id, int tweetId);

    }
}
