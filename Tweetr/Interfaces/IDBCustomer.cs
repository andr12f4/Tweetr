using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetr.Models;

namespace Tweetr.Interfaces
{
    public interface IDBCustomer
    {
        public bool Create(Customer customer);
        public Customer GetCustomer(int id);
        public Customer GetCustomer(string username, string password);
        Customer DeleteCustomer(int id);
        public bool Update(int id, Customer customer);
        public List<Customer> GetAllCustomers();

    }
}

