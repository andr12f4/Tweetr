using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tweetr.Data;
using Tweetr.Interfaces;
using Tweetr.Models;

namespace Tweetr.Services
{
    public class TweetHandler : ITweet
    {

        private string _filePath = Path.GetFullPath("./Data/TweetData.json", Environment.CurrentDirectory);

        public void Create(Tweet tweet)
        {
            Dictionary<int, Tweet> dicT = new JsonFile<Tweet>().ReadJsonFile(_filePath);
            dicT.Add(tweet.Id, tweet);
            new JsonFile<Tweet>().WriteJsonFile(dicT, _filePath);
        }

        public void DeleteTweet(int id)
        {
            Dictionary<int, Tweet> dicT = new JsonFile<Tweet>().ReadJsonFile(_filePath);
            dicT.Remove(id);
            new JsonFile<Tweet>().WriteJsonFile(dicT, _filePath);
        }

        public List<Tweet> GetAllFriendsTweets(int userID)
        {
            Dictionary<int, Tweet> dicT = new JsonFile<Tweet>().ReadJsonFile(_filePath);
            List<int> listofFriends = new JsonFile<Customer>().ReadJsonFile(_filePath)[userID].Friends;
            List<Tweet> listOfTweets = new List<Tweet>();
            foreach (Tweet tweet in dicT.Values)
            {
                for (int i = 0; i < listofFriends.Count; i++){

                if (tweet.customer.Id == listofFriends[i])
                    {
                        listOfTweets.Add(tweet);
                    }
                }
            }
            return listOfTweets;
        }

        public List<Tweet> GetAllPrivateTweets(int userID)
        {
            Dictionary<int, Tweet> dicT = new JsonFile<Tweet>().ReadJsonFile(_filePath);
            
            List<Tweet> listOfTweets = new List<Tweet>();
            foreach (Tweet tweet in dicT.Values)
            {
                if (tweet.customer.Id == userID)
                {
                   listOfTweets.Add(tweet);
                }
                
            }
            return listOfTweets;
        }

        public List<Tweet> GetAllPublicTweets()
        {
            Dictionary<int, Tweet> dicT = new JsonFile<Tweet>().ReadJsonFile(_filePath);
            List<Tweet> listOfTweets = new List<Tweet>();
            foreach (Tweet tweet in dicT.Values)
            {
                if (tweet.tweetPublicity == TweetPublicity.Public)
                {
                    listOfTweets.Add(tweet);
                }

            }
            return listOfTweets;
        }

        public Tweet GetTweet(int id)
        {
            Dictionary<int, Tweet> dicT = new JsonFile<Tweet>().ReadJsonFile(_filePath);
            return dicT[id];
        }
    }
}
