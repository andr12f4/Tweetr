using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tweetr.Pages.Login
{
    public class LoginBrugerListeModel : PageModel
    {

        public Dictionary<int, Models.Customer> Liste { get; set; }
        public void OnGet()
        {
            Liste = new Services.CustomerHandler().GetDictionary();
        }
    }
}
