using System;
using MadOrderingssystem.Models;
using MadOrderingssystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MadOrderingssystem.Pages.Login
{
    /*
     * Lavet af:    Andreas
     * Bidraget af: 
    */
    public class DeleteUserModel : PageModel
    {
        [BindProperty]
        public Customer CustomerSession { get; set; }

        [BindProperty]
        public Customer Customer { get; set; }


        public IActionResult OnGet(string id)
        {
            CustomerHandler cH = new CustomerHandler();
            Customer = cH.Get(id);
            try
            {
                CustomerSession = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
            }
            catch (ArgumentNullException ex) { }
            return Page();
        }

        public IActionResult OnPost()
        {
            CustomerHandler cH = new CustomerHandler();
            cH.Delete(Customer.ID);

            return RedirectToPage("LoginList");
        }
    }
}
