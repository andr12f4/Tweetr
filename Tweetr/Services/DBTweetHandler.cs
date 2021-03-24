using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Tweetr.Interfaces;
using Tweetr.Models;

namespace Tweetr.Services
{
    public class DBTweetHandler : ITweet
    {
        private const String ConnString = @"Data Source=alek0532.database.windows.net;Initial Catalog=Tweetr;User ID=trifunovic;Password=Zealand1303;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private const String createTweet = "insert into Tweets (Id, Text, DOT, TPublicity, " +
                                           "Username) Values (@ID, @TEXT, @DOT, @TPUBLICITY)";

        private const String updateTweet = "Update Tweet set Id = @ID," +
                                          "Text=@TEXT," +
                                          "Tpublicity=@TPUBLICITY where Id = @ID";

        public void Create(Tweet tweet)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(createTweet, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", tweet.Id);
                    cmd.Parameters.AddWithValue("@TEXT", tweet.Text);
                    cmd.Parameters.AddWithValue("@TPUBLICITY", tweet.tweetPublicity);
                    cmd.Parameters.AddWithValue("@DOT", tweet.DateOfTweet);


                    int rows = cmd.ExecuteNonQuery();


                }
            }
        }

        public void DeleteTweet(int id)
        {
            Tweet u = GetTweet(id);

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("delete from Tweets where Id = @ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    int rows = cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Tweet> GetAllFriendsTweets(int userID)
        {
            List<Tweet> tweets = new List<Tweet>();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Tweets.Id, Tweets.Tweet ,Tweets.DOB, Tweets.Tpublicity, Tweets.customerId, Likes.UserID FROM Tweets Right JOIN Likes ON Tweets.Id = Likes.TweetId WHERE Tweets.customerId = @ID", conn))
                {

                    SqlDataReader reader = cmd.ExecuteReader();
                    cmd.Parameters.AddWithValue("@ID", userID);
                    Tweet u = new Tweet();
                    u.Likes = new List<int>();
                    while (reader.Read())
                    {
                        if (u.Id == reader.GetInt32(0))
                        {

                            u.Id = reader.GetInt32(0);
                            u.Text = reader.GetString(1);
                            u.DateOfTweet = reader.GetDateTime(2);
                            u.tweetPublicity = reader.GetInt32(3);
                            u.customer.Id = reader.GetInt32(4);
                            u.Likes.Add(reader.GetInt32(5));

                        }
                        else
                        {
                            tweets.Add(u);
                            u = new Tweet();
                            u.Likes = new List<int>();
                        }
                    }
                   List<int> listOfFriends = new DBCustomerHandler().GetCustomer(userID).Friends;
            List<Tweet> listOfTweets = new List<Tweet>();
            foreach (Tweet tweet in tweets)
            {
                for (int i = 0; i < listOfFriends.Count; i++)
                {

                    if (tweet.customer.Id == listOfFriends[i])
                    {
                        listOfTweets.Add(tweet);
                    }
                }
            }
            return listOfTweets;

                }

                throw new KeyNotFoundException("Der var ingen tweets ");
            }

            
        }

        public List<Tweet> GetAllPrivateTweets(int UserID)
        {
            List<Tweet> tweets = new List<Tweet>();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Tweets.Id, Tweets.Tweet ,Tweets.DOB, Tweets.Tpublicity, Tweets.customerId, Likes.UserID FROM Tweets Right JOIN Likes ON Tweets.Id = Likes.TweetId WHERE Tweets.customerId = @ID", conn))
                {

                    SqlDataReader reader = cmd.ExecuteReader();
                    cmd.Parameters.AddWithValue("@ID", UserID);
                    Tweet u = new Tweet();
                    u.Likes = new List<int>();
                    while (reader.Read())
                    {
                        if (u.Id == reader.GetInt32(0))
                        {
                            
                            u.Id = reader.GetInt32(0);
                            u.Text = reader.GetString(1);
                            u.DateOfTweet = reader.GetDateTime(2);
                            u.tweetPublicity = reader.GetInt32(3);
                            u.customer.Id = reader.GetInt32(4);
                            u.Likes.Add(reader.GetInt32(5));

                        }
                        else
                        {
                            tweets.Add(u);
                            u = new Tweet();
                            u.Likes = new List<int>();
                        }
                    }
                    return tweets;

                }

                throw new KeyNotFoundException("Der var ingen tweets ");
            }
        }

        public List<Tweet> GetAllPublicTweets()
        {
            List<Tweet> tweets = new List<Tweet>();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Tweets.Id, Tweets.Tweet ,Tweets.DOB, Tweets.Tpublicity, Tweets.customerId, Likes.UserID FROM Tweets Right JOIN Likes ON Tweets.Id = Likes.TweetId WHERE Tweets.Id = @Id", conn))
                {

                    SqlDataReader reader = cmd.ExecuteReader();
                    Tweet u = new Tweet();
                    u.Likes = new List<int>();
                    while (reader.Read())
                    {
                        if (u.Id == reader.GetInt32(0))
                        {
                        u.Id = reader.GetInt32(0);
                        u.Text = reader.GetString(1);
                        u.DateOfTweet = reader.GetDateTime(2);
                        u.tweetPublicity = reader.GetInt32(3);
                        u.customer.Id = reader.GetInt32(4);
                        u.Likes.Add(reader.GetInt32(5));

                        } else
                        {
                            tweets.Add(u);
                            u = new Tweet();
                            u.Likes = new List<int>();
                        }
                    }
                    
                }

                throw new KeyNotFoundException("Der var ingen tweets ");
            }
        }

        public Tweet GetTweet(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Tweets.Id, Tweets.Tweet ,Tweets.DOB, Tweets.Tpublicity, Tweets.customerId, Likes.UserID FROM Tweets Right JOIN Likes ON Tweets.Id = Likes.TweetId WHERE Tweets.Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Tweet u = new Tweet();
                    u.Likes = new List<int>();
                    while (reader.Read())
                    {
                        u.Id = reader.GetInt32(0);
                        u.Text = reader.GetString(1);
                        u.DateOfTweet = reader.GetDateTime(2);
                        u.tweetPublicity = reader.GetInt32(3);
                        u.customer.Id = reader.GetInt32(4);
                        u.Likes.Add(reader.GetInt32(5));
                    }
                    return u;
                }

                throw new KeyNotFoundException("Der var ingen tweet med id = " + id);
            }
        }

        public void UpdateTweet(int id, Tweet tweet)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(updateTweet, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", tweet.Id);
                    cmd.Parameters.AddWithValue("@TEXT", tweet.Text);
                    cmd.Parameters.AddWithValue("@TPUBLICITY", tweet.tweetPublicity);
                    cmd.Parameters.AddWithValue("@DOT", tweet.DateOfTweet);


                    int rows = cmd.ExecuteNonQuery();


                }
            }
        }
    }
}
