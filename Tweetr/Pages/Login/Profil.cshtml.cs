using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MadOrderingssystem.Models;
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
    public class ProfilModel : PageModel
    {
    public Customer CustomerSession { get; set; }
    public string exeptionMSG { get; set; }
        public void OnGet()
        {
            try
            {
            CustomerSession = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
            } catch(ArgumentNullException ex) { exeptionMSG = "Du er ikke logget ind"; }
            
        }
    }
}
