using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Tweetr.Interfaces;
using Tweetr.Models;

namespace Tweetr.Services
{
    public class DBCustomerHandler : ICustomer
    {
        private const String ConnString = @"Data Source=alek0532.database.windows.net;Initial Catalog=Tweetr;User ID=trifunovic;Password=Zealand1303;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private const String createUser = "insert into Users (Name, Email, Password, " +
                                          "Username) Values (@NAME, @EMAIL, @PASSWORD, @USERNAME)";

        private const String updateUser = "Update Users set Id=@ID," +
                                          "Name=@NAME," +
                                          "Email=@EMAIL," +
                                          "Password=@PASSWORD," +
                                          "Username=@USERNAME where Id = @ID";

        public void AddFriend(int id, string username)
        {
            Customer customer1 = new Customer();
            foreach(Customer customer in GetAllCustomers().Values)
            {
                if (customer.Username == username)
                {
                    customer1 = customer;
                }
            }
            if (customer1 != null)
            {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("insert into Friends(Id, FriendID) Values (@Id, @idFriend)", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@idFriend", customer1.Id);

                    cmd.ExecuteNonQuery();
                }
            }

            }
        }

        public void Create(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(createUser, conn))
                {
                    //cmd.Parameters.AddWithValue("@ID", customer.Id);
                    cmd.Parameters.AddWithValue("@NAME", customer.Name);
                    cmd.Parameters.AddWithValue("@EMAIL", customer.Email);
                    cmd.Parameters.AddWithValue("@PASSWORD", customer.Password);
                    cmd.Parameters.AddWithValue("@USERNAME", customer.Username);
                    int rows = cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCustomer(int id)
        {
            Customer u = GetCustomer(id);

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("delete from Users where Id = @ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    int rows = cmd.ExecuteNonQuery();
                }
            }

            
        }

        public Dictionary<int,Customer> GetAllCustomers()
        {
            Dictionary<int, Customer> list = new Dictionary<int, Customer>();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("Select * FROM Users", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Customer u = new Customer();
                        u.Id = reader.GetInt32(0);
                        u.Name = reader.GetString(1);
                        u.Email = reader.GetString(2);
                        u.Password = reader.GetString(3);
                        u.Username = reader.GetString(4);


                        list.Add(u.Id,u);

                    }
                }

                return list;
            }
        }

        public Customer GetCustomer(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Users.Id, Users.Name,Users.Username, Users.Password,Users.Email, Friends.FriendID FROM Users Right JOIN Friends ON Users.Id = Friends.Id Where Users.Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    Customer u = new Customer();
                    u.Friends = new List<int>();
                    while (reader.Read())
                    {
                        u.Id = reader.GetInt32(0);
                        u.Name = reader.GetString(1);
                        u.Email = reader.GetString(4);
                        u.Password = reader.GetString(3);
                        u.Username = reader.GetString(2);
                        u.Friends.Add(reader.GetInt32(5));
                    }
                    return u;
                }

                throw new KeyNotFoundException("Der var ingen userstory med id = " + id);
            }
        }



        public Customer GetCustomer(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Users.Id, Users.Name,Users.Username, Users.Password,Users.Email," +
                                                       " Friends.FriendID FROM Users Right JOIN Friends ON Users.Id = Friends.Id Where Users.Username = @user And Users.Password = @pass", conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);
                    SqlDataReader reader = cmd.ExecuteReader();
                        Customer u = new Customer();
                        u.Friends = new List<int>();
                    while (reader.Read())
                    {
                        u.Id = reader.GetInt32(0);
                        u.Name = reader.GetString(1);
                        u.Email = reader.GetString(4);
                        u.Password = reader.GetString(3);
                        u.Username = reader.GetString(2);
                        u.Friends.Add(reader.GetInt32(5));
                    }
                    return u;
                }

                throw new KeyNotFoundException("Der var ingen userstory med username = " + username);
            }
        }

        public void Update(int id, Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(updateUser, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", customer.Id);
                    cmd.Parameters.AddWithValue("@NAME", customer.Name);
                    cmd.Parameters.AddWithValue("@EMAIL", customer.Email);
                    cmd.Parameters.AddWithValue("@PASSWORD", customer.Password);
                    cmd.Parameters.AddWithValue("@USERNAME", customer.Username);


                    int rows = cmd.ExecuteNonQuery();

                   
                }
            }
        }
    }
}
