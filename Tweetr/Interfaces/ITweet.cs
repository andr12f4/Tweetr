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
        public void DeleteTweet(int id);
        public List<Tweet> GetAllPublicTweets();
        public List<Tweet> GetAllFriendsTweets(int userID);
        public List<Tweet> GetAllPrivateTweets(int UserID);

    }
}
