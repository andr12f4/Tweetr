using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tweetr.Interfaces;
using Tweetr.Models;

namespace Tweetr.Pages
{
    public class MyProfileModel : PageModel
    {
        private ICustomer _customerHandler;

        public Customer customer { get; set; }

        public MyProfileModel(ICustomer IC)
        {
            _customerHandler = IC;
        }
        public void OnGet()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
               
            }
        }

        public IActionResult OnPostAddFriend(string username)
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
                Customer customerFriend = new Customer();
                foreach (Customer cus in _customerHandler.GetAllCustomers())
                {
                    if (cus.Username == username)
                    {
                        customerFriend = cus;
                    }
                }
                
                customer.Friends.Add(customerFriend.Id);
                _customerHandler.Update(customer.Id, customer);
                
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteFriend(string username)
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
                Customer customerFriend = new Customer();
                foreach (Customer cus in _customerHandler.GetAllCustomers())
                {
                    if (cus.Username == username)
                    {
                        customerFriend = cus;
                    }
                }

                customer.Friends.Remove(customerFriend.Id);
                _customerHandler.Update(customer.Id, customer);

            }

            return RedirectToPage();
        }
    }
}
