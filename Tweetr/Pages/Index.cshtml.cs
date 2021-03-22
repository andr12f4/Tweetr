using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Tweetr.Interfaces;
using Tweetr.Models;

namespace Tweetr.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private ITweet _tweetHandler;
        
        
        public Customer customer { get; set; }
        public bool NotLoggedIn { get; set; }
        [BindProperty]
        public List<Tweet> TweetsPublic { get; set; }
        [BindProperty]
        public List<Tweet> TweetsPrivate { get; set; }
        [BindProperty]
        public Tweet Tweet { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ITweet iTweet)
        {
            _logger = logger;
            _tweetHandler = iTweet;
        }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
                TweetsPrivate = _tweetHandler.GetAllFriendsTweets(customer.Id);
            }
            else
            {
                NotLoggedIn = true;
            }

            TweetsPublic = _tweetHandler.GetAllPublicTweets();
        }

        public IActionResult OnPost()
        {
            if (Tweet.Text != null)
            {
                Tweet.customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
                
                _tweetHandler.Create(Tweet);
                return RedirectToPage();
            }
            return Page();
        }

        public IActionResult OnPostLikes(int id)
        {
            Tweet tweet = _tweetHandler.GetTweet(id);
            if (TweetsPublic != null && TweetsPrivate != null)
            {
                tweet.Likes.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user")).Id);
            }
            
            _tweetHandler.UpdateTweet(id,tweet);
            return RedirectToPage();
        }
        public IActionResult OnPostDelete(int id)
        {
            _tweetHandler.DeleteTweet(id);
            return RedirectToPage();
        }
    }
}
