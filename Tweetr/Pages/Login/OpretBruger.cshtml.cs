using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Tweetr.Interfaces;
using Tweetr.Models;

namespace Tweetr.Pages.Login
{
    public class OpretBrugerModel : PageModel
    {

        [BindProperty]
        public Customer Customer { get; set; }

        private ICustomer CustomerHandler;

        public OpretBrugerModel(ICustomer ic)
        {
            CustomerHandler = ic;
        } 

        public void OnGet()
        {
            
          

        }

        public IActionResult OnPost()
        {

            if (Customer != null)
            {
                Customer.Friends = new List<int>();
                CustomerHandler.Create(Customer);
                return RedirectToPage("/Index");
            }

            return Page();
        }

      
    }
}
