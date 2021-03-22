using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Tweetr.Interfaces;
using Tweetr.Models;

namespace Tweetr.Services
{
    public class DBCustomerHandler : IDBCustomer
    {
        private const String ConnString = @"Data Source=alek0532.database.windows.net;Initial Catalog=Tweetr;User ID=trifunovic;Password=Zealand1303;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private const String createUser = "insert into Users (Id, Name, Email, Password, " +
                                          "Username) Values (@ID, @NAME, @EMAIL, @PASSWORD, @USERNAME)";

        private const String updateUser = "Update Users set Id=@ID," +
                                          "Name=@NAME," +
                                          "Email=@EMAIL," +
                                          "Password=@PASSWORD," +
                                          "Username=@USERNAME where Id = @ID";

        public List<Customer> List;


        //CREATE TABLE Users(
        //    Id  int NOT NULL PRIMARY KEY,
        //Name VARCHAR(30)      NOT NULL,
        //    Email   VARCHAR(50)   NOT NULL,
        //    Password VARCHAR(30) NOT NULL,
        //    Username VARCHAR(30) NOT NULL
        //);

        public bool Create(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(createUser, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", customer.Id);
                    cmd.Parameters.AddWithValue("@NAME", customer.Name);
                    cmd.Parameters.AddWithValue("@EMAIL", customer.Email);
                    cmd.Parameters.AddWithValue("@PASSWORD", customer.Password);
                    cmd.Parameters.AddWithValue("@USERNAME", customer.Username);



                    int rows = cmd.ExecuteNonQuery();

                    return rows == 1;
                }
            }
        }

        public Customer DeleteCustomer(int id)
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

            return u;
        }

        public List<Customer> GetAllCustomers()
        {
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


                        List.Add(u);

                    }
                }

                return List;
            }
        }

        public Customer GetCustomer(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("select * from Users where Id = @ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Customer u = new Customer();
                        u.Id = reader.GetInt32(0);
                        u.Name = reader.GetString(1);
                        u.Email = reader.GetString(2);
                        u.Password = reader.GetString(3);
                        u.Username = reader.GetString(4);
                        return u;
                    }
                }

                throw new KeyNotFoundException("Der var ingen userstory med id = " + id);
            }
        }

        public Customer GetCustomer(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, Customer customer)
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

                    return rows == 1;
                }
            }
        }
    }
}
