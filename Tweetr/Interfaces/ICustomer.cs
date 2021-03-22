using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetr.Models;

namespace Tweetr.Interfaces
{
    public interface ICustomer
    {
        public void Create(Customer customer);
        public Customer GetCustomer(int id);
        public Customer GetCustomer(string username, string password);
        public void DeleteCustomer(int id);
        public void Update(int id, Customer customer);
        public Dictionary<int, Customer> GetAllCustomers();

        public void AddFriend(int id, string FriendUsername);

    }
}
