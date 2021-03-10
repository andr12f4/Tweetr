using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Tweetr.Models;

namespace Tweetr.Pages.Login
{
    public class OpretBrugerModel : PageModel
    {
        public Customer CustomerSession { get; set; }

        [BindProperty]
        public Customer Customer { get; set; }

        public void OnGet()
        {
            try
            {
                CustomerSession = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
            }
            catch (ArgumentNullException ex) { }

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                Customer.CustomerDiscount = true;
                CustomerHandler cH = new CustomerHandler();
                cH.Create(Customer);

                return RedirectToPage("/Index");
            }
            return Page();

        }
    }
}
