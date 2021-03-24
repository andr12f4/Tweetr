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

        private const String createTweet = "insert into Tweets (Tweet, Tpublicity, DOB, " +
                                           "customerId) Values (@TEXT, @TPUBLICITY, @DOT, @username)";

        private const String updateTweet = "Update Tweet set Id = @ID," +
                                          "Tweet=@TEXT," +
                                          "Tpublicity=@TPUBLICITY, DOB=@dob,customerId=@cId where Id = @ID";

        public void Create(Tweet tweet)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(createTweet, conn))
                {
                    
                    cmd.Parameters.AddWithValue("@TEXT", tweet.Text);
                    cmd.Parameters.AddWithValue("@TPUBLICITY", tweet.tweetPublicity);
                    cmd.Parameters.AddWithValue("@DOT", tweet.DateOfTweet);
                    cmd.Parameters.AddWithValue("@username", tweet.customer.Id);


                    cmd.ExecuteNonQuery();


                }
            }
        }

        public void DeleteTweet(int id)
        {
            Tweet u = GetTweet(id);

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("delete from Tweets where Id = @ID ", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    // kan ikke delete når der er foreign keys, spørg peter
                    int rows = cmd.ExecuteNonQuery();
                }
            }
        }

        public Dictionary<int,Tweet> GetAllFriendsTweets(int userID)
        {
            
            Dictionary<int, Tweet> tweets = new Dictionary<int, Tweet>();
            Dictionary<int, Tweet> resultOfTweets = new Dictionary<int, Tweet>();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Tweets.Id, Tweets.Tweet ,Tweets.DOB, Tweets.Tpublicity, Tweets.customerId, Likes.UserID, Likes.TweetId FROM Tweets LEFT JOIN Likes ON Tweets.Id = Likes.TweetId", conn))
                {

                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    Tweet u = new Tweet();
                    u.Likes = new List<int>();
                    while (reader.Read())
                    {
                        if (tweets.ContainsKey(reader.GetInt32(0)))
                        {
                            if (!reader.IsDBNull(5))
                            {
                                tweets[reader.GetInt32(0)].Likes.Add(reader.GetInt32(5));
                            }
                        }
                        else
                        {
                            u = new Tweet();
                            u.Likes = new List<int>();
                            u.Id = reader.GetInt32(0);
                            u.Text = reader.GetString(1);
                            u.DateOfTweet = reader.GetDateTime(2);
                            u.tweetPublicity = (TweetPublicity)reader.GetInt32(3);
                            u.customer = new Customer();
                            u.customer.Id = reader.GetInt32(4);
                            if (!reader.IsDBNull(5))
                            {
                                u.Likes.Add(reader.GetInt32(5));
                            }

                            tweets.Add(u.Id, u);

                        }

                    }

                }

                Dictionary<int, Customer> allCustomers = new DBCustomerHandler().GetAllCustomers();

                foreach (Tweet tweet in tweets.Values)
                {

                    foreach (Customer customer in allCustomers.Values)
                    {
                        if (tweet.customer.Id == customer.Id)
                        {
                            tweet.customer.Name = customer.Name;
                            tweet.customer.Username = customer.Username;
                        }
                    }
                }

                Customer customers = new DBCustomerHandler().GetCustomer(userID);
                foreach (Tweet tweet in tweets.Values)
                {

                    if (customers.Friends.Contains(tweet.customer.Id))
                    {
                        resultOfTweets.Add(tweet.Id, tweet);
                    }
                }
            }

            
            return resultOfTweets;

                throw new KeyNotFoundException("Der var ingen tweets ");
            }

        

        public Dictionary<int,Tweet> GetAllPrivateTweets(int UserID)
        {
            Dictionary<int, Tweet> tweets = new Dictionary<int, Tweet>();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * From Tweets WHERE Tweets.customerId = @ID", conn))
                {

                    cmd.Parameters.AddWithValue("@ID", UserID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Tweet u = new Tweet();
                    u.Likes = new List<int>();
                    while (reader.Read())
                    {
                        
                            u = new Tweet();
                            u.Likes = new List<int>();
                            u.Id = reader.GetInt32(0);
                            u.Text = reader.GetString(1);
                            u.DateOfTweet = reader.GetDateTime(3);
                            u.tweetPublicity = (TweetPublicity)reader.GetInt32(2);
                            u.customer = new Customer();
                            u.customer.Id = reader.GetInt32(4);
                            if (!reader.IsDBNull(4))
                            {
                                u.Likes.Add(reader.GetInt32(4));
                            }

                            tweets.Add(u.Id, u);

                        }

                    
                }
                return tweets;
                throw new KeyNotFoundException("Der var ingen tweets ");
            }
        }

        public Dictionary<int,Tweet> GetAllPublicTweets()
        {
            Dictionary<int, Tweet> tweets = new Dictionary<int,Tweet>();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Tweets.Id, Tweets.Tweet ,Tweets.DOB, Tweets.Tpublicity, Tweets.customerId, Likes.UserID, Likes.TweetId FROM Tweets LEFT JOIN Likes ON Tweets.Id = Likes.TweetId", conn))
                {

                    SqlDataReader reader = cmd.ExecuteReader();
                    
                        Tweet u = new Tweet();
                        
                    while (reader.Read())
                    {
                        if (tweets.ContainsKey(reader.GetInt32(0)))
                        {
                            if (!reader.IsDBNull(5))
                            {
                                tweets[reader.GetInt32(0)].Likes.Add(reader.GetInt32(5));
                            }
                        }
                        else
                        {
                            u = new Tweet();
                            u.Likes = new List<int>();
                            u.Id = reader.GetInt32(0);
                            u.Text = reader.GetString(1);
                            u.DateOfTweet = reader.GetDateTime(2);
                            u.tweetPublicity = (TweetPublicity)reader.GetInt32(3);
                            u.customer = new Customer();
                            u.customer.Id = reader.GetInt32(4);
                            if (!reader.IsDBNull(5))
                            {
                                u.Likes.Add(reader.GetInt32(5));
                            }
                            if (u.tweetPublicity == TweetPublicity.Public)
                            {
                                tweets.Add(u.Id, u);
                            }
                        }
                            
                    }

                    Dictionary<int,Customer> allCustomers = new DBCustomerHandler().GetAllCustomers();
                
                    foreach (Tweet tweet in tweets.Values)
                    {
                    
                        foreach (Customer customer in allCustomers.Values)
                        {
                            if (tweet.customer.Id == customer.Id)
                            {
                                tweet.customer.Name = customer.Name;
                                tweet.customer.Username = customer.Username;
                            }
                        }
                    }
                }
                    return tweets;

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
                        u.tweetPublicity = (TweetPublicity) reader.GetInt32(3);
                        u.customer = new Customer();
                        u.customer.Id = reader.GetInt32(4);
                        u.Likes.Add(reader.GetInt32(5));
                    }
                    return u;
                }

                throw new KeyNotFoundException("Der var ingen tweet med id = " + id);
            }
        }


        public void AddLike(int id,int tweetId)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("insert into Likes (TweetId, UserID) Values (@ID, @cId)", conn))
                {

                    

                    cmd.Parameters.AddWithValue("@ID", tweetId);
                    cmd.Parameters.AddWithValue("@cId", id);
                    int rows = cmd.ExecuteNonQuery();

                }
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
                    cmd.Parameters.AddWithValue("@cId", tweet.customer.Id);

                    int rows = cmd.ExecuteNonQuery();


                }
            }
        }
    }
}
