using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tweetr.Interfaces;
using Tweetr.Models;
using Tweetr.Services;


namespace Tweetr.Pages
{
    public class MineTweetsModel : PageModel
    {
        public Dictionary<int,Models.Tweet> Liste { get; set; }

        private ITweet _tweetHandler;

        public MineTweetsModel(ITweet IT)
        { 
            _tweetHandler = IT;
        }
        public void OnGet(int id)
        {
            
            if (HttpContext.Session.GetString("user") != null)
            {
                Customer customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
                Liste = _tweetHandler.GetAllPrivateTweets(customer.Id);
            }
           
        }

        public IActionResult OnPostDelete(int id)
        {
            _tweetHandler.DeleteTweet(id);
            return RedirectToPage();
        }
    }
}
