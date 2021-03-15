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
using Tweetr.Services;

namespace Tweetr.Pages.Login
{
    public class Log_inModel : PageModel
    {
        [BindProperty]
        public Customer Customer { get; set; }
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        private ICustomer customerHandler;

        public Log_inModel(ICustomer ic)
        {
            customerHandler = ic;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {

            Customer = customerHandler.GetCustomer(Username, Password);
            if (Customer != null)
            {
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(Customer));
                return RedirectToPage("/Index");
            }
           
            return Page();
        }
    }
}
