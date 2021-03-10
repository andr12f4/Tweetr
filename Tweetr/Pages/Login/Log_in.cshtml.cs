using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tweetr.Models;

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

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {

            CustomerHandler customerHandler = new CustomerHandler();
            Customer = customerHandler.Get(Username, Password);
            if (Customer != null)
            {
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(Customer));
                return RedirectToPage("/Index");
            }
            if (!ModelState.IsValid) { return Page(); }
            return Page();
        }
    }
}
