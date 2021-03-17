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
    public class CustomerHandler : ICustomer
    {
        private string _filePath = Path.GetFullPath("./Data/UserData.json", Environment.CurrentDirectory);

        public void Create(Customer customer)
        {
            Dictionary<int, Customer> dicT = new JsonFile<Customer>().ReadJsonFile(_filePath);
            customer.Id = dicT.Count + 1;
            dicT.Add(dicT.Count + 1, customer);
            new JsonFile<Customer>().WriteJsonFile(dicT, _filePath);
        }

        public void DeleteCustomer(int id)
        {
            Dictionary<int, Customer> dicT = new JsonFile<Customer>().ReadJsonFile(_filePath);
            dicT.Remove(id);
            new JsonFile<Customer>().WriteJsonFile(dicT, _filePath);
        }

        public List<Customer> GetAllCustomers()
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int id)
        {
            Dictionary<int, Customer> dicT = new JsonFile<Customer>().ReadJsonFile(_filePath);
            return dicT[id];
        }
        public Customer GetCustomer(string username, string password)
        {
            Dictionary<int, Customer> dicT = new JsonFile<Customer>().ReadJsonFile(_filePath);
            foreach (Customer customer in dicT.Values)
            {
                if (customer.Username == username && customer.Password == password)
                {
                    return customer;
                }
            }
            return null;
        }
        public Dictionary<int, Customer> GetDictionary()
        {
            JsonFile<Customer> jsonCustomer = new JsonFile<Customer>();
            return jsonCustomer.ReadJsonFile(_filePath);

        }

        public void Update(int id, Customer customer)
        {
            Dictionary<int, Customer> dicT = new JsonFile<Customer>().ReadJsonFile(_filePath);
            dicT[id] = customer;
            new JsonFile<Customer>().WriteJsonFile(dicT, _filePath);
        }

    }

}

